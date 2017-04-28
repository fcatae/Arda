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
    public class WorkController : Controller
    {
        public IActionResult Index()
        {
            var user = User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            ViewBag.User = user;
            
            UsageTelemetry.Track(user, ArdaUsage.Work_Index);

            return View();
        }

        [HttpGet("[controller]/{workspace}")]
        public IActionResult Workspace(string workspace)
        {
            var user = User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            ViewBag.User = user;
            ViewBag.Title = workspace.ToUpper();
            ViewBag.Work = workspace.ToLower();

            UsageTelemetry.Track(user, ArdaUsage.Work_Index);

            return View();
        }
    }
}
