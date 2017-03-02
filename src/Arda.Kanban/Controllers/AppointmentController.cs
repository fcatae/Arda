using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using Arda.Common.ViewModels.Main;
using System.Net;
using Arda.Kanban.Models.Repositories;

namespace Arda.Kanban.Controllers
{
    [Route("api/[controller]")]
    public class AppointmentController : Controller
    {
        IAppointmentRepository _repository;

        public AppointmentController(IAppointmentRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Add()
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(HttpContext.Request.Body);
            string requestFromPost = reader.ReadToEnd();
            var appointment = JsonConvert.DeserializeObject<AppointmentViewModel>(requestFromPost);

            // Calling update
            var response = _repository.AddNewAppointment(appointment);

            if (response)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("list")]
        public IEnumerable<AppointmentViewModel> List()
        {
            var appointments = _repository.GetAllAppointments();

            return appointments;
        }

        [HttpGet]
        [Route("listfromuser")]
        public IEnumerable<AppointmentViewModel> ListFromUser([FromQuery]string user)
        {
            var appointments = _repository.GetAllAppointments(user);

            return appointments;
        }

        [HttpGet]
        [Route("getappointmentbyid")]
        public AppointmentViewModel GetAppointmentByID([FromQuery]Guid id)
        {
            var appointment = _repository.GetAppointmentByID(id);

            return appointment;
        }
        
        [HttpPut]
        [Route("editappointment")]
        public HttpResponseMessage EditAppointmentByID()
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(HttpContext.Request.Body);
            string requestFromPost = reader.ReadToEnd();
            var appointment = JsonConvert.DeserializeObject<AppointmentViewModel>(requestFromPost);


            var response = _repository.EditAppointment(appointment);

            if (response)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [Route("deleteappointmentbyid")]
        public HttpResponseMessage DeleteAppointmentByID([FromQuery]Guid id)
        {
            var response = _repository.DeleteAppointmentByID(id);

            if (response)
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
