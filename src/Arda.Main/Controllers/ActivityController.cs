using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Arda.Common.Utils;
using System.Net.Http;
using Arda.Main.ViewModels;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Arda.Main.Controllers
{
    [Authorize]
    public class ActivityController : Controller
    {
        [HttpGet]
        public async Task<JsonResult> GetActivities()
        {
            // Getting uniqueName
            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            // Getting the response of remote service
            var activity = await Util.ConnectToRemoteService<List<ActivityViewModel>>(HttpMethod.Get, Util.KanbanURL + "api/activity/list", uniqueName, "");

            return Json(activity);
        }
    }
}
