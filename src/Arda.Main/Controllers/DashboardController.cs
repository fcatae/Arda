using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Arda.Common.Utils;
using System.Net.Http;
using Arda.Main.Utils;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Net.Http.Headers;
using Arda.Main.ViewModels;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Arda.Main.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {

        public async Task<IActionResult> Index()
        {
            var user = User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
            var userStatus = Util.ConnectToRemoteService<int>(HttpMethod.Get, Util.PermissionsURL + "api/useroperations/getuserstatus", user, string.Empty).Result;
            string token = null;

            ViewBag.User = user;
            ViewBag.UserStatus = userStatus;

            if (!Startup.IsSimpleAuthForDemo)
            {
                Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationResult result = await TokenManager.GetAccessToken(HttpContext);
                token = result.AccessToken;
                ViewBag.Token = token;
            }

            if (userStatus == 0)
            {
                StoreUserInfo(user, token);
            }

            UsageTelemetry.Track(user, ArdaUsage.Dashboard_Index);

            return View();
        }


        private async void StoreUserInfo(string user, string token)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage responseProfile = await client.SendAsync(request);

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
            }
        }
    }
}
