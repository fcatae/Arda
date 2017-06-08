using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Arda.Common.JSON;
using Arda.Common.Utils;
using System.Net;
using Arda.Main.ViewModels;

namespace Arda.Main.Controllers
{
    [Authorize]
    public class MetricController : Controller
    {

        #region Views

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            ViewBag.Guid = Util.GenerateNewGuid().ToString();
            return View();
        }

        public IActionResult Details(Guid id)
        {
            // Getting uniqueName
            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            // Getting the selected fiscal year
            var metricToBeViewed = Util.ConnectToRemoteService<MetricViewModel>(HttpMethod.Get, Util.KanbanURL + "api/metric/getmetricbyid?id=" + id, uniqueName, "").Result;

            if (metricToBeViewed != null)
            {
                var metric = new MetricViewModel()
                {
                    MetricID = metricToBeViewed.MetricID,
                    MetricCategory = metricToBeViewed.MetricCategory,
                    MetricName = metricToBeViewed.MetricName,
                    Description = metricToBeViewed.Description,
                    FiscalYearID = metricToBeViewed.FiscalYearID,
                    TextualFiscalYear = metricToBeViewed.TextualFiscalYear,
                    FullNumericFiscalYear = metricToBeViewed.FullNumericFiscalYear
                };

                return View(metric);
            }
            else
            {
                ViewBag.Message = "The system has not found the metric.";
                return View("Error");
            }
        }

        public IActionResult Edit(Guid id)
        {
            // Getting uniqueName
            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            // Getting the selected fiscal year
            var metricToBeViewed = Util.ConnectToRemoteService<MetricViewModel>(HttpMethod.Get, Util.KanbanURL + "api/metric/getmetricbyid?id=" + id, uniqueName, "").Result;

            if (metricToBeViewed != null)
            {
                var metric = new MetricViewModel()
                {
                    MetricID = metricToBeViewed.MetricID,
                    MetricCategory = metricToBeViewed.MetricCategory,
                    MetricName = metricToBeViewed.MetricName,
                    Description = metricToBeViewed.Description,
                    FiscalYearID = metricToBeViewed.FiscalYearID,
                    TextualFiscalYear = metricToBeViewed.TextualFiscalYear,
                    FullNumericFiscalYear = metricToBeViewed.FullNumericFiscalYear
                };

                return View(metric);
            }
            else
            {
                ViewBag.Message = "The system has not found the metric.";
                return View("Error");
            }
        }

        #endregion

        #region Actions

        [HttpGet]
        public async Task<JsonResult> ListAllMetrics()
        {
            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            SourceDataTablesFormat dataTablesSource = new SourceDataTablesFormat();

            var existentMetrics = await Util.ConnectToRemoteService<List<MetricViewModel>>(HttpMethod.Get, Util.KanbanURL + "api/metric/list", uniqueName, "");

            foreach (MetricViewModel m in existentMetrics)
            {
                IList<string> dataRow = new List<string>();
                dataRow.Add(m.TextualFiscalYear.ToString());
                dataRow.Add(m.MetricCategory.ToString());
                dataRow.Add(m.MetricName.ToString());
                dataRow.Add($"<div class='data-sorting-buttons'><a href='/metric/details/{m.MetricID}' class='ds-button-detail'><i class='fa fa-align-justify' aria-hidden='true'></i> Details</a></div>&nbsp;<div class='data-sorting-buttons'><a href='/metric/edit/{m.MetricID}' class='ds-button-edit'><i class='fa fa-pencil-square-o' aria-hidden='true'></i> Edit</a></div>&nbsp;<div class='data-sorting-buttons'><a data-toggle='modal' data-target='#generic-modal' onclick=\"ModalDelete_Metric('{m.MetricID}','{m.MetricCategory}','{m.MetricName}');\" class='ds-button-delete'><i class='fa fa-trash' aria-hidden='true'></i> Delete</a></div>");
                dataTablesSource.aaData.Add(dataRow);
            }

            return Json(dataTablesSource);
        }

        [HttpGet]
        public async Task<JsonResult> GetMetrics()
        {
            // Getting uniqueName
            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            // Getting the response of remote service
            var metrics = await Util.ConnectToRemoteService<List<MetricViewModel>>(HttpMethod.Get, Util.KanbanURL + "api/metric/listbyyear?year=" + DateTime.Now.Year, uniqueName, "");

            return Json(metrics);
        }

        [HttpGet]
        public async Task<JsonResult> ListAllCategories()
        {
            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            var categories = new List<string>();

            var existentMetrics = await Util.ConnectToRemoteService<List<MetricViewModel>>(HttpMethod.Get, Util.KanbanURL + "api/metric/list", uniqueName, "");

            foreach (MetricViewModel m in existentMetrics)
            {
                if (!categories.Contains(m.MetricCategory))
                {
                    categories.Add(m.MetricCategory);
                }
            }

            return Json(categories);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> AddMetric(MetricViewModel metric)
        {
            if (metric == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            var responseAboutUpdate = await Util.ConnectToRemoteService(HttpMethod.Post, Util.KanbanURL + "api/metric/add", uniqueName, "", metric);

            if (responseAboutUpdate.IsSuccessStatusCode)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        public async Task<HttpResponseMessage> EditMetric(MetricViewModel metric)
        {
            if (metric == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            var responseAboutUpdate = await Util.ConnectToRemoteService(HttpMethod.Put, Util.KanbanURL + "api/metric/editmetricbyid", uniqueName, "", metric);

            if (responseAboutUpdate.IsSuccessStatusCode)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteMetric(Guid id)
        {
            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            var fiscalYearToBeDeleted = await Util.ConnectToRemoteService(HttpMethod.Delete, Util.KanbanURL + "api/metric/deletemetricbyid?id=" + id, uniqueName, "", id);

            if (fiscalYearToBeDeleted.IsSuccessStatusCode)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        #endregion


        #region Utils


        #endregion

    }
}
