using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Arda.Common.Interfaces.Kanban;
using System.Net.Http;
using Newtonsoft.Json;
using Arda.Common.ViewModels.Main;
using System.Net;

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
            try
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
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("list")]
        public IEnumerable<AppointmentViewModel> List()
        {
            try
            {
                var appointments = _repository.GetAllAppointments();

                if (appointments != null)
                {
                    return appointments;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("listfromuser")]
        public IEnumerable<AppointmentViewModel> ListFromUser([FromQuery]string user)
        {
            try
            {
                var appointments = _repository.GetAllAppointments(user);

                if (appointments != null)
                {
                    return appointments;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("getappointmentbyid")]
        public AppointmentViewModel GetAppointmentByID([FromQuery]Guid id)
        {
            try
            {
                var appointment = _repository.GetAppointmentByID(id);

                if (appointment != null)
                {
                    return appointment;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        [HttpPut]
        [Route("editappointment")]
        public HttpResponseMessage EditAppointmentByID()
        {
            try
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
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [Route("deleteappointmentbyid")]
        public HttpResponseMessage DeleteAppointmentByID([FromQuery]Guid id)
        {
            try
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
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
