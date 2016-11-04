using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Arda.Common.Utils;
using System.Net.Http;
using Arda.Common.ViewModels.Reports;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Arda.Reports.Controllers
{
    [Route("api/[controller]")]
    public class ActivityController : Controller
    {
        // GET: /<controller>/
        [HttpGet]
        [Route("bubble")]
        public async Task<D3BubbleViewModel> Bubble(DateTime startDate, DateTime endDate, string user = "All")
        {
            var data = await Util.ConnectToRemoteService<IEnumerable<ActivityConsumingViewModel>>(HttpMethod.Get, Util.KanbanURL + "api/Report/getactivityconsumingdata?startDate=" + startDate + "&endDate=" + endDate + "&user=" +user, "","");

            var bubble = new D3BubbleViewModel()
            {
                name = "ActivityConsuming",
                children = new List<D3BubbleChild0>()
            };

            foreach (var d in data)
            {
                bubble.children.Add(new D3BubbleChild0()
                {
                    name = d.Activity,
                    children = new List<D3BubbleChild1>()
                    {
                        new D3BubbleChild1() {name = d.Activity, size = d.Hours }
                    }
                });
            }
            
            return bubble;
        }

        [HttpGet]
        [Route("tabledata")]
        public async Task<IEnumerable<ActivityConsumingViewModel>> TableData(DateTime startDate, DateTime endDate, string user = "All")
        {
            var data = await Util.ConnectToRemoteService<IEnumerable<ActivityConsumingViewModel>>(HttpMethod.Get, Util.KanbanURL + "api/Report/getactivityconsumingdata?startDate=" + startDate + "&endDate=" + endDate + "&user=" + user, "", "");

            return data;
        }
    }
}
