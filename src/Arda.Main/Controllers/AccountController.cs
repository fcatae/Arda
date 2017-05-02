using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Arda.Common.Utils;
using System.Net.Http;
using System.Security.Claims;
using System.Collections.Generic;
using Arda.Main.Utils;
using Arda.Main.ViewModels;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Arda.Main.Controllers
{
    public class AccountController : Controller
    {
        public async Task<bool> AuthSimple(string name, string email)
        {
            var claims = new List<Claim>
                    {
                        new Claim("sub", "1"),
                        new Claim("name", name),
                        new Claim("email", email),
                        new Claim("status", "Online"),
                        new Claim("department", "Evangelism"),
                        new Claim("region", "Brazil"),
                        new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", name),
                        new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", email)
                    };

            var id = new ClaimsIdentity(claims, "local", "name", "role");

            await HttpContext.Authentication.SignInAsync("defaultCookieAuth", new ClaimsPrincipal(id));

            return true;
        }

        [HttpGet]
        public IActionResult Page()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Page(string username, string email)
        {
            await AuthSimple(username, email); 

            return Redirect("/AuthCompleted");
        }

        public async Task<IActionResult> LoginAD()
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

        public IActionResult AuthCompleted()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            if(Startup.IsSimpleAuthForDemo)
            {
                return Redirect("/Account/page");
            }

            return new ChallengeResult(
                OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties { RedirectUri = Util.MainURL + "Return" });
        }

        public async Task<IActionResult> SignOut()
        {
            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
            await Util.ConnectToRemoteService(HttpMethod.Delete, Util.PermissionsURL + "api/permission/deleteuserpermissions", uniqueName, "");

            var callbackUrl = Url.Action("SignOutCallback", "Account", values: null, protocol: Request.Scheme);

            if(Startup.IsSimpleAuthForDemo)
            {
                await HttpContext.Authentication.SignOutAsync("defaultCookieAuth");

                return Redirect(callbackUrl);
            }
            else
            {
                await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.Authentication.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme,
                    new AuthenticationProperties { RedirectUri = callbackUrl });
            }

            return new EmptyResult();
        }

        public IActionResult SignOutCallback()
        {
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

    }
}
