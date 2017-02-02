using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Arda.Common.ViewModels.Reports;
using Arda.Common.Utils;
using System.Net.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Arda.Reports.Controllers
{
    [Route("api/[controller]")]
    public class MetricController : Controller
    {
        [HttpGet]
        [Route("tabledata")]
        public async Task<IEnumerable<MetricConsumingViewModel>> TableData([FromQuery]DateTime startDate, [FromQuery]DateTime endDate, [FromQuery]string user = "All")
        {
            var data = await Util.ConnectToRemoteService<IEnumerable<MetricConsumingViewModel>>(HttpMethod.Get, Util.KanbanURL + "api/Report/getmetricconsumingdata?startDate=" + startDate + "&endDate=" + endDate + "&user=" + user, "", "");

            return data;
        }
    }
}
