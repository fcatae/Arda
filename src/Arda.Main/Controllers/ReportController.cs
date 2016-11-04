using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Arda.Common.Utils;
using Arda.Common.ViewModels.Reports;
using System.Net.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Arda.Main.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        public IActionResult TimeConsuming()
        {
            return View();
        }

        public async Task<JsonResult> GetActivityConsumingBubbleData(DateTime startDate, DateTime endDate, string user = "All")
        {
            var data = await Util.ConnectToRemoteService<D3BubbleViewModel>(HttpMethod.Get, Util.ReportsURL + "api/Activity/bubble?startDate=" + startDate + "&endDate=" + endDate + "&user=" + user, "", "");
            return Json(data);
        }

        public async Task<JsonResult> GetActivityConsumingTableData(DateTime startDate, DateTime endDate, string user = "All")
        {
            var data = await Util.ConnectToRemoteService<IEnumerable<ActivityConsumingViewModel>>(HttpMethod.Get, Util.ReportsURL + "api/Activity/tabledata?startDate=" + startDate + "&endDate=" + endDate + "&user=" + user, "", "");
            return Json(data);
        }

        public async Task<JsonResult> GetExpertiseConsumingBubbleData(DateTime startDate, DateTime endDate, string user = "All")
        {
            var data = await Util.ConnectToRemoteService<D3BubbleViewModel>(HttpMethod.Get, Util.ReportsURL + "api/Expertise/bubble?startDate=" + startDate + "&endDate=" + endDate + "&user=" + user, "", "");
            return Json(data);
        }

        public async Task<JsonResult> GetExpertiseConsumingTableData(DateTime startDate, DateTime endDate, string user = "All")
        {
            var data = await Util.ConnectToRemoteService<IEnumerable<ExpertiseConsumingViewModel>>(HttpMethod.Get, Util.ReportsURL + "api/Expertise/tabledata?startDate=" + startDate + "&endDate=" + endDate + "&user=" + user, "", "");
            return Json(data);
        }

        public async Task<JsonResult> GetMetricConsumingTableData(DateTime startDate, DateTime endDate, string user = "All")
        {
            var data = await Util.ConnectToRemoteService<IEnumerable<MetricConsumingViewModel>>(HttpMethod.Get, Util.ReportsURL + "api/Metric/tabledata?startDate=" + startDate + "&endDate=" + endDate + "&user=" + user, "", "");
            return Json(data);
        }
    }
}
