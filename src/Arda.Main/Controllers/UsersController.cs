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
            var photo = Util.GetUserPhoto(userID);
            ViewBag.Photo = photo;

            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
            var user = await Util.ConnectToRemoteService<UserMainViewModel>(HttpMethod.Get, Util.PermissionsURL + "api/useroperations/getuser?uniqueName=" + userID, uniqueName, "");
            return View(user);
        }

        //Edit User:
        public async Task<IActionResult> Edit(string userID)
        {
            var photo = Util.GetUserPhoto(userID);
            ViewBag.Photo = photo;

            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
            var user = await Util.ConnectToRemoteService<UserMainViewModel>(HttpMethod.Get, Util.PermissionsURL + "api/useroperations/getuser?uniqueName=" + userID, uniqueName, "");
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

        public string GetUserPhoto(string user)
        {
            var photo = Util.GetUserPhoto(user);
            return photo;
        }
        
        [ResponseCache(Duration=600)]
        [HttpGet("users/photo/{user}")]
        public string GetUserPhotoFromCache(string user)
        {
            var photo = Util.GetUserPhoto(user);
            return photo;
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
