using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Arda.Common.Utils;
using System.Net.Http;
using System.Net;
using Arda.Common.JSON;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using Arda.Main.ViewModels;

namespace Arda.Main.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        public IActionResult Add()
        {
            ViewBag.Guid = Util.GenerateNewGuid();
            return View();
        }

        [HttpPost]
        public async Task<HttpResponseMessage> AddAppointment(AppointmentViewModel appointment)
        {
            if (appointment == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            // Converting the T&E value to Decimal before save process
            decimal TE = 0;
            Decimal.TryParse(Request.Form["_AppointmentTE"], NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, new CultureInfo("pt-BR"), out TE);
            appointment._AppointmentTE = TE;

            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            var responseAboutAdd = await Util.ConnectToRemoteService(HttpMethod.Post, Util.KanbanURL + "api/appointment/add", uniqueName, "", appointment);

            UsageTelemetry.Track(uniqueName, ArdaUsage.Appointment_Add);

            if (responseAboutAdd.IsSuccessStatusCode)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public IActionResult My()
        {
            return View();
        }

        public IActionResult All()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> ListMyAppointments()
        {
            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            SourceDataTablesFormat dataTablesSource = new SourceDataTablesFormat();

            var existentAppointments = await Util.ConnectToRemoteService<List<AppointmentViewModel>>(HttpMethod.Get, Util.KanbanURL + "api/appointment/listfromuser?=" + uniqueName, uniqueName, "");

            foreach (AppointmentViewModel a in existentAppointments)
            {
                IList<string> dataRow = new List<string>();
                dataRow.Add(a._WorkloadTitle.ToString());
                dataRow.Add(a._AppointmentDate.ToString("dd/MM/yyyy"));
                dataRow.Add(a._AppointmentHoursDispensed.ToString());
                dataRow.Add($"<div class='data-sorting-buttons'><a href='/appointment/details/{a._AppointmentID}' class='ds-button-detail'><i class='fa fa-align-justify' aria-hidden='true'></i> Details</a></div>&nbsp;<div class='data-sorting-buttons'><a href='/appointment/edit/{a._AppointmentID}' class='ds-button-edit'><i class='fa fa-pencil-square-o' aria-hidden='true'></i> Edit</a></div>&nbsp;<div class='data-sorting-buttons'><a href='#' onclick=\"detailsWorkloadState(); HideAllButtons(); loadWorkload('{a._AppointmentWorkloadWBID}');\" data-toggle='modal' data-target='#WorkloadModal' class='ds-button-workload'><i class='fa fa-tasks' aria-hidden='true'></i> Workload</a></div>&nbsp;<div class='data-sorting-buttons'><a data-toggle='modal' data-target='#generic-modal' onclick=\"ModalDelete_Appointment('{a._AppointmentID}','{a._WorkloadTitle}','{a._AppointmentDate.ToString("dd/MM/yyyy")}','{a._AppointmentHoursDispensed}','{a._AppointmentUserUniqueName}');\" class='ds-button-delete'><i class='fa fa-trash' aria-hidden='true'></i> Delete</a></div>");
                dataTablesSource.aaData.Add(dataRow);
            }

            return Json(dataTablesSource);
        }

        [HttpGet]
        public async Task<JsonResult> ListAllAppointments()
        {
            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            SourceDataTablesFormat dataTablesSource = new SourceDataTablesFormat();

            var existentAppointments = await Util.ConnectToRemoteService<List<AppointmentViewModel>>(HttpMethod.Get, Util.KanbanURL + "api/appointment/list", uniqueName, "");

            foreach (AppointmentViewModel a in existentAppointments)
            {
                IList<string> dataRow = new List<string>();
                dataRow.Add(a._WorkloadTitle.ToString());
                dataRow.Add(a._AppointmentDate.ToString("dd/MM/yyyy"));
                dataRow.Add(a._AppointmentHoursDispensed.ToString());
                dataRow.Add(Util.GetUserAlias(a._AppointmentUserUniqueName.ToString()));
                dataRow.Add($"<div class='data-sorting-buttons'><a href='/appointment/details/{a._AppointmentID}' class='ds-button-detail'><i class='fa fa-align-justify' aria-hidden='true'></i> Details</a></div>&nbsp;<div class='data-sorting-buttons'><a href='/appointment/edit/{a._AppointmentID}' class='ds-button-edit'><i class='fa fa-pencil-square-o' aria-hidden='true'></i> Edit</a></div>&nbsp;<div class='data-sorting-buttons'><a href='#' onclick=\"detailsWorkloadState(); HideAllButtons(); loadWorkload('{a._AppointmentWorkloadWBID}');\" data-toggle='modal' data-target='#WorkloadModal' class='ds-button-workload'><i class='fa fa-tasks' aria-hidden='true'></i> Workload</a></div>&nbsp;<div class='data-sorting-buttons'><a data-toggle='modal' data-target='#generic-modal' onclick=\"ModalDelete_Appointment('{a._AppointmentID}','{a._WorkloadTitle}','{a._AppointmentDate.ToString("dd/MM/yyyy")}','{a._AppointmentHoursDispensed}','{a._AppointmentUserUniqueName}');\" class='ds-button-delete'><i class='fa fa-trash' aria-hidden='true'></i> Delete</a></div>");
                dataTablesSource.aaData.Add(dataRow);
            }

            return Json(dataTablesSource);
        }

        public IActionResult Details(Guid id)
        {
            // Getting uniqueName
            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            // Getting the selected appointment
            var appointmentToBeViewed = Util.ConnectToRemoteService<AppointmentViewModel>(HttpMethod.Get, Util.KanbanURL + "api/appointment/getappointmentbyid?id=" + id, uniqueName, "").Result;

            if (appointmentToBeViewed != null)
            {
                return View(appointmentToBeViewed);
            }
            else
            {
                ViewBag.Message = "The system has not found the requested appointment.";
                return View("Error");
            }
        }

        public IActionResult Edit(Guid id)
        {
            // Getting uniqueName
            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            // Getting the selected appointment
            var appointmentToBeViewed = Util.ConnectToRemoteService<AppointmentViewModel>(HttpMethod.Get, Util.KanbanURL + "api/appointment/getappointmentbyid?id=" + id, uniqueName, "").Result;

            if (appointmentToBeViewed != null)
            {
                return View(appointmentToBeViewed);
            }
            else
            {
                ViewBag.Message = "The system has not found the requested appointment.";
                return View("Error");
            }
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteAppointment(Guid id)
        {
            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            var appointmentToBeDeleted = await Util.ConnectToRemoteService(HttpMethod.Delete, Util.KanbanURL + "api/appointment/deleteappointmentbyid?id=" + id, uniqueName, "", id);

            if (appointmentToBeDeleted.IsSuccessStatusCode)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        public async Task<HttpResponseMessage> EditAppointment(AppointmentViewModel appointment)
        {
            if (appointment == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            // Converting the T&E value to Decimal before save process
            decimal TE = 0;
            Decimal.TryParse(Request.Form["_AppointmentTE"], NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, new CultureInfo("pt-BR"), out TE);
            appointment._AppointmentTE = TE;

            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            var responseAboutUpdate = await Util.ConnectToRemoteService(HttpMethod.Put, Util.KanbanURL + "api/appointment/editappointment", uniqueName, "", appointment);

            if (responseAboutUpdate.IsSuccessStatusCode)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
