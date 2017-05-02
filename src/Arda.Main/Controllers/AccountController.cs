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

            return Redirect("/Dashboard");
        }

        public IActionResult SignIn()
        {
            if(Startup.IsSimpleAuthForDemo)
            {
                return Redirect("/Account/page");
            }

            return new ChallengeResult(
                OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties { RedirectUri = Util.MainURL + "Dashboard" });
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
