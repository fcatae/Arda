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
    public class UserFormController : Controller
    {
        [HttpGet]
        public IActionResult New(string template)
        {
            var user = this.GetCurrentUserName();

            ViewBag.Title = "Title";

            switch (template.ToLower())
            {
                case "event":
                    ViewBag.Title = "Event Name";
                    ViewBag.ReportName = "_sampleEventReport";
                    break;
                case "customer":
                    ViewBag.Title = "Report Title";
                    ViewBag.ReportName = "_sampleCustomerReport";
                    break;
            }

            UsageTelemetry.Track(user, ArdaUsage.Userform_Index);

            return View();
        }        

        [HttpPost]
        public IActionResult Submit([FromForm] object objForm)
        {
            var user = this.GetCurrentUserName();

            dynamic userForm = objForm;

            string title = userForm.workloadTitle;
            string date = userForm.workloadDate;
            string text = userForm.workloadDescription;

            UsageTelemetry.Track(user, ArdaUsage.Userform_Submit);

            return RedirectToAction("Index", "Dashboard");
        }
    }
}
