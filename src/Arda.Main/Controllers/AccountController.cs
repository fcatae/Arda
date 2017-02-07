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
        private IDistributedCache _cache;

        public AccountController(IDistributedCache cache)
        {
            _cache = cache;
        }
        
        public async Task<bool> AuthSimple()
        {
            var claims = new List<Claim>
                    {
                        new Claim("sub", "1"),
                        new Claim("name", "User 1"),
                        new Claim("email", "user@ardademo.onmicrosoft.com"),
                        new Claim("status", "Online"),
                        new Claim("department", "Evangelism"),
                        new Claim("region", "Brazil"),
                        //new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "user@ardademo.onmicrosoft.com"),
                        new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "user@ardademo.onmicrosoft.com"),
                        new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", "ardademo")
                    };

            var id = new ClaimsIdentity(claims, "local", "name", "role");

            await HttpContext.Authentication.SignInAsync("defaultCookieAuth", new ClaimsPrincipal(id));

            return true;
        }

        public async Task<IActionResult> SignIn()
        {
            if(Startup.IsSimpleAuthForDemo)
            {
                await AuthSimple();

                return Redirect(Util.MainURL + "Dashboard");
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
