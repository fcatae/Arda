using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using Arda.Common.Utils;
using Arda.Common.ViewModels.Main;
using Newtonsoft.Json;
using Arda.Common.JSON;
using System.Net;

namespace Arda.Main.Controllers
{
    [Authorize]
    public class FiscalYearController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        // List all fiscal years in database.
        public JsonResult ListAllFiscalYears()
        {
            // Getting uniqueName
            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            // Creating the final expected object to datatable
            SourceDataTablesFormat dataTablesSource = new SourceDataTablesFormat();

            try
            {
                // Getting the response of remote service
                var existentFiscalYears = Util.ConnectToRemoteService<List<FiscalYearViewModel>>(HttpMethod.Get, Util.KanbanURL + "api/fiscalyear/list", uniqueName, "").Result;

                // Mouting rows data
                foreach (FiscalYearViewModel fy in existentFiscalYears)
                {
                    IList<string> dataRow = new List<string>();
                    dataRow.Add(fy.TextualFiscalYearMain.ToString());
                    dataRow.Add(fy.FullNumericFiscalYearMain.ToString());
                    dataRow.Add($"<div class='data-sorting-buttons'><a href='/fiscalyear/details/{fy.FiscalYearID}' class='ds-button-detail'><i class='fa fa-align-justify' aria-hidden='true'></i>Details</a></div><div class='data-sorting-buttons'><a href='/fiscalyear/edit/{fy.FiscalYearID}' class='ds-button-edit'><i class='fa fa-pencil-square-o' aria-hidden='true'></i>Edit</a></div><div class='data-sorting-buttons'><a href='javascript:void()' data-toggle='modal' data-target='#generic-modal' onclick=\"ModalDelete_FiscalYear('{fy.FiscalYearID}','{fy.TextualFiscalYearMain}');\" class='ds-button-delete'><i class='fa fa-trash' aria-hidden='true'></i>Delete</a></div>");
                    dataTablesSource.aaData.Add(dataRow);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return Json(dataTablesSource);
        }

        // Mount details page based in fiscal year ID
        public IActionResult Details(Guid id)
        {
            try
            {
                // Getting uniqueName
                var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

                // Getting the selected fiscal year
                var fiscalYearToBeViewed = Util.ConnectToRemoteService<FiscalYearViewModel>(HttpMethod.Get, Util.KanbanURL + "api/fiscalyear/getfiscalyearbyid?id=" + id, uniqueName, "").Result;

                if (fiscalYearToBeViewed != null)
                {
                    var finalFiscalYear = new FiscalYearViewModel()
                    {
                        FiscalYearID = fiscalYearToBeViewed.FiscalYearID,
                        FullNumericFiscalYearMain = fiscalYearToBeViewed.FullNumericFiscalYearMain,
                        TextualFiscalYearMain = fiscalYearToBeViewed.TextualFiscalYearMain
                    };

                    return View(finalFiscalYear);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception)
            {
                return View();
            }
        }

        // Mount edit fiscal year page based on ID
        public IActionResult Edit(Guid id)
        {
            try
            {
                // Getting uniqueName
                var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

                // Getting the selected fiscal year
                var fiscalYearToBeViewed = Util.ConnectToRemoteService<FiscalYearViewModel>(HttpMethod.Get, Util.KanbanURL + "api/fiscalyear/getfiscalyearbyid?id=" + id, uniqueName, "").Result;

                if (fiscalYearToBeViewed != null)
                {
                    var finalFiscalYear = new FiscalYearViewModel()
                    {
                        FiscalYearID = fiscalYearToBeViewed.FiscalYearID,
                        FullNumericFiscalYearMain = fiscalYearToBeViewed.FullNumericFiscalYearMain,
                        TextualFiscalYearMain = fiscalYearToBeViewed.TextualFiscalYearMain
                    };

                    return View(finalFiscalYear);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception)
            {
                return View();
            }
        }

        // Update the fiscal year in database
        [HttpPost]
        public JsonResult EditFiscalYear(FiscalYearViewModel fiscalyear)
        {
            if (fiscalyear == null)
            {
                return Json(new { Status = "Fail" });
            }

            // Getting uniqueName
            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            // Getting the selected fiscal year
            var responseAboutUpdate = Util.ConnectToRemoteService(HttpMethod.Put, Util.KanbanURL + "api/fiscalyear/editfiscalyearbyid", uniqueName, "", fiscalyear).Result;

            if (responseAboutUpdate.IsSuccessStatusCode)
            {
                return Json(new { Status = "Ok" });
            }
            else
            {
                return Json(new { Status = "Fail" });
            }
        }

        // Deleting fiscal year from system
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            try
            {
                // Getting uniqueName
                var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

                // Getting the selected fiscal year
                var fiscalYearToBeDeleted = Util.ConnectToRemoteService(HttpMethod.Delete, Util.KanbanURL + "api/fiscalyear/deletefiscalyearbyid?id=" + id, uniqueName, "", id).Result;

                if (fiscalYearToBeDeleted.IsSuccessStatusCode)
                {
                    return Json(new { Status = "Ok" });
                }
                else
                {
                    return Json(new { Status = "Fail" });
                }
            }
            catch (Exception)
            {
                return Json(new { Status = "Fail" });
            }
        }

        // Mounting fiscal year screen
        public IActionResult Add()
        {
            ViewBag.Guid = Util.GenerateNewGuid().ToString();
            return View();
        }

        public JsonResult AddFiscalYear(FiscalYearViewModel fiscalYear)
        {
            if (fiscalYear == null)
            {
                return Json(new { Status = "Fail" });
            }

            // Getting uniqueName
            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            // Getting the selected fiscal year
            var responseAboutUpdate = Util.ConnectToRemoteService(HttpMethod.Post, Util.KanbanURL + "api/fiscalyear/addfiscalyear", uniqueName, "", fiscalYear).Result;

            if (responseAboutUpdate.IsSuccessStatusCode)
            {
                return Json(new { Status = "Ok" });
            }
            else
            {
                return Json(new { Status = "Fail" });
            }
        }
    }
}
