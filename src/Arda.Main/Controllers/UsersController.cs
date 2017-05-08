using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using Arda.Common.Utils;
using System.Net;
using Arda.Common.JSON;
using Arda.Main.ViewModels;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.ApplicationInsights;
using System;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.IO;

//TODO: Refactor name Users to User
namespace Arda.Main.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {

        private static IDistributedCache _cache;

        public UsersController(IDistributedCache cache)
        {
            _cache = cache;
        }

        #region Views

        //All Users
        public IActionResult Index()
        {
            return View();
        }

        //Review Permissions:
        public IActionResult Review()
        {
            return View();
        }

        //User Details:
        public async Task<IActionResult> Details(string userID)
        {
            var photo = Util.GetUserPhotoString(userID);
            ViewBag.Photo = photo; // embedded picture
            
            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
            var user = await Util.ConnectToRemoteService<UserMainViewModel>(HttpMethod.Get, Util.PermissionsURL + "api/useroperations/getuser?uniqueName=" + userID, uniqueName, "");
            return View(user);
        }

        //Edit User:
        public async Task<IActionResult> Edit(string userID)
        {
            ViewBag.Photo = null;

            try
            {
                // it MAY fail when user has no picture
                var photo = Util.GetUserPhotoString(userID);
                ViewBag.Photo = photo; // Embedded picture
            }
            catch
            {                
            }

            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
            var user = await Util.ConnectToRemoteService<UserMainViewModel>(HttpMethod.Get, Util.PermissionsURL + "api/useroperations/getuser?uniqueName=" + userID, uniqueName, "");

            ViewBag.User = userID;
            
            return View(user);            
        }

        #endregion

        #region Actions

        //From Permissions:
        public async Task<JsonResult> ViewRestrictedUserList()
        {
            // Getting uniqueName
            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            bool isAdmin = true;

            var existentUsers = await Util.ConnectToRemoteService<List<UserMainViewModel>>(HttpMethod.Get, Util.PermissionsURL + "api/useroperations/getusers", uniqueName, "");

            if (!isAdmin)
            {
                existentUsers.Clear();
            }

            return Json(existentUsers);
        }

        public async Task<JsonResult> ListAllUsers()
        {
            // Getting uniqueName
            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            // Creating the final expected object to datatable
            SourceDataTablesFormat dataTablesSource = new SourceDataTablesFormat();

            // Getting the response of remote service
            var existentUsers = await Util.ConnectToRemoteService<List<UserMainViewModel>>(HttpMethod.Get, Util.PermissionsURL + "api/useroperations/getusers", uniqueName, "");

            // Mouting rows data
            foreach (UserMainViewModel user in existentUsers)
            {
                IList<string> dataRow = new List<string>();
                dataRow.Add(user.Name.ToString());
                dataRow.Add(user.Email.ToString());
                dataRow.Add(getUserSituation(user.Status));
                dataRow.Add($"<div class='data-sorting-buttons'><a href='/users/details?userID={user.Email}' class='ds-button-detail'><i class='fa fa-align-justify' aria-hidden='true'></i>Details</div></a><div class='data-sorting-buttons'><a href='/users/edit?userID={user.Email}' class='ds-button-edit'><i class='fa fa-pencil-square-o' aria-hidden='true'></i>Edit</a></div><div class='data-sorting-buttons'><a data-toggle='modal' data-target='#DeleteUserModal' onclick=\"ModalDeleteUser('{user.Email}','{user.Name}');\" class='ds-button-delete'><i class='fa fa-trash' aria-hidden='true'></i>Delete</a></div>");
                dataTablesSource.aaData.Add(dataRow);
            }

            return Json(dataTablesSource);
        }

        public async Task<JsonResult> ListPendingUsers()
        {
            var uniqueName = User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            SourceDataTablesFormat dataTablesSource = new SourceDataTablesFormat();

            var users = await Util.ConnectToRemoteService<List<UserMainViewModel>>(HttpMethod.Get, Util.PermissionsURL + "api/useroperations/getpendingusers", uniqueName, "");

            // Mouting rows data
            foreach (UserMainViewModel user in users)
            {
                IList<string> dataRow = new List<string>();
                dataRow.Add(user.Name.ToString());
                dataRow.Add(user.Email.ToString());
                dataRow.Add($"<div class=\"data-sorting-buttons\"><a onclick=\"ModalSelectUser('{user.Email}','{user.Name}');\" class=\"ds-button-detail\"><i class=\"fa fa-align-justify\" aria-hidden=\"true\"></i>Details</a></div>");
                dataTablesSource.aaData.Add(dataRow);
            }

            return Json(dataTablesSource);
        }

        //From Kanban:
        public async Task<JsonResult> GetUsers()
        {
            // Getting uniqueName
            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            // Getting the response of remote service
            var users = await Util.ConnectToRemoteService<List<UserKanbanViewModel>>(HttpMethod.Get, Util.KanbanURL + "api/user/list", uniqueName, "");

            return Json(users);
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(string user)
        {
            if (user != null)
            {
                var uniqueName = User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
                var response = await Util.ConnectToRemoteService(HttpMethod.Delete, Util.PermissionsURL + "api/permission/deleteuser?uniqueName=" + user, uniqueName, "");

                if (response.IsSuccessStatusCode)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }
            }
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        [HttpPut]
        public async Task<HttpResponseMessage> UpdatePermissions([FromQuery] string user, [FromBody] PermissionsViewModel permissions)
        {
            if (user != null && permissions != null)
            {
                var uniqueName = User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
                var response = await Util.ConnectToRemoteService(HttpMethod.Put, Util.PermissionsURL + "api/permission/updateuserpermissions?uniqueName=" + user, uniqueName, "", permissions);

                if (response.IsSuccessStatusCode)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }
            }
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        [HttpPut]
        public async Task<string> RefreshPhoto(string uniqueName)
        {
            var currentUniqueName = User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
            string name = uniqueName ?? currentUniqueName;

            string token;

            Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationResult result = await Utils.TokenManager.GetAccessToken(HttpContext);
            token = result.AccessToken;

            return await StoreUserPhoto(name, token);
        }
        
        private async Task<string> StoreUserPhoto(string name, string token)
        {
            string url = $"https://graph.microsoft.com/v1.0/users/{name}/photo/$value";

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Authorization", "bearer " + token);

            var response = await client.SendAsync(request);

            byte[] content = await response.Content.ReadAsByteArrayAsync();

            return await PhotoUpdateInternal(name, content);                        
        }

        [HttpPut]
        public async Task<string> RefreshUserInfo(string uniqueName)
        {
            var currentUniqueName = User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            if (uniqueName != currentUniqueName)
                throw new InvalidOperationException("StoreUserInfo currently does not impersonate the user. It uses /me");

            string name = uniqueName ?? currentUniqueName;

            string token;

            Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationResult result = await Utils.TokenManager.GetAccessToken(HttpContext);
            token = result.AccessToken;

            return await StoreUserInfo(name, token);
        }

        [HttpGet]
        public async Task<string> StoreUserInfo(string user, string token)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage responseProfile = await client.SendAsync(request);
            string result = null;

            HttpRequestMessage requestManager = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me/manager");
            requestManager.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseManager = await client.SendAsync(requestManager);

            Task.WaitAll();

            if (responseProfile.IsSuccessStatusCode && responseManager.IsSuccessStatusCode)
            {
                var profileSerialized = await responseProfile.Content.ReadAsStringAsync();
                var profile = JsonConvert.DeserializeObject<GraphProfileViewModel>(profileSerialized);
                var managerSerialized = await responseManager.Content.ReadAsStringAsync();
                var manager = JsonConvert.DeserializeObject<GraphProfileViewModel>(managerSerialized);

                var userToBeUpdated = new UserMainViewModel()
                {
                    Name = profile.displayName,
                    Email = profile.userPrincipalName,
                    GivenName = profile.givenName,
                    Surname = profile.surname,
                    JobTitle = profile.jobTitle,
                    ManagerUniqueName = manager.userPrincipalName
                };

                var userStatus = Util.ConnectToRemoteService(HttpMethod.Put, Util.PermissionsURL + "api/permission/updateuser?=" + user, user, string.Empty, userToBeUpdated).Result;

                result = profileSerialized;
            }

            return result;
        }

        private async Task<string> PhotoUpdateInternal(string uniqueName, byte[] content)
        {
            string img = "data:image/jpeg;base64," + Convert.ToBase64String(content);

            var response = await Util.ConnectToRemoteService(HttpMethod.Put, Util.PermissionsURL + "api/permission/updateuserphoto?uniqueName=" + uniqueName, uniqueName, string.Empty, img);

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException("api/permission/updateuserphoto failed");
            }

            return img;
        }

        [HttpPut]
        public async Task<HttpResponseMessage> PhotoUpdate(string img)
        {
            if (img != null)
            {
                var uniqueName = User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
                var response = await Util.ConnectToRemoteService(HttpMethod.Put, Util.PermissionsURL + "api/permission/updateuserphoto?uniqueName=" + uniqueName, uniqueName, string.Empty, img);

                if (response.IsSuccessStatusCode)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }
            }
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        public async Task<JsonResult> GetAllResources()
        {
            var uniqueName = User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
            var items = await Util.ConnectToRemoteService<List<ResourcesViewModel>>(HttpMethod.Get, Util.PermissionsURL + "api/permission/getallpermissions", uniqueName, "");
            return Json(items);
        }

        public async Task<JsonResult> GetUserPermissions(string user)
        {
            if (user != null)
                return null;

            var uniqueName = User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
            var permissions = await Util.ConnectToRemoteService<PermissionsViewModel>(HttpMethod.Get, Util.PermissionsURL + "/api/UserOperations/GetUserPermissions?uniqueName=" + user, uniqueName, "");
            return Json(permissions);            
        }

        // Should not be called
        [Obsolete]
        public string GetUserPhoto(string user)
        {
            throw new InvalidOperationException("GetUserPhoto is obsolete");

            //var photo = Util.GetUserPhotoString(user);
            //return photo;
        }

        [ResponseCache(Duration=600)]
        [HttpGet("users/photo/{user}")]
        public IActionResult GetUserPhotoFromCache(string user)
        {
            try
            {
                string data = Util.GetUserPhotoString(user);

                string[] components = data.Split(',');

                int typePhotoStart = components[0].IndexOf(":") + 1;
                int typePhotoEnd = components[0].IndexOf(";");
                string encodedPhoto = components[1];

                string typePhoto = components[0].Substring(typePhotoStart, typePhotoEnd - typePhotoStart);
                byte[] binaryPhoto = Convert.FromBase64String(encodedPhoto);
                var streamPhoto = new MemoryStream(binaryPhoto);

                return new FileStreamResult(streamPhoto, typePhoto);
            }
            catch(TaskCanceledException ex)
            {
                // This exception is very common - keep this line until we figure out what is going on 
                var cacheTimeoutException = new System.InvalidOperationException("GetUserPhotoFromCache: thrown taskCanceledException - cache is not populated?", ex);

                var client = new TelemetryClient();
                client.TrackException(cacheTimeoutException);

                throw cacheTimeoutException;
            }
        }
        
        #endregion

        #region Utils

        private string getUserSituation(int situation)
        {
            switch (situation)
            {
                case 0:
                    return "Waiting Review";
                case 1:
                    return "Permissions Denied";
                case 2:
                    return "Permissions Granted";
                default:
                    return "";
            }
        }

        #endregion

    }
}
