using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Arda.Common.ViewModels.Main;
using Arda.Common.Interfaces.Kanban;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;

namespace Arda.Kanban.Controllers
{
    [Route("api/[controller]")]
    public class WorkloadController : Controller
    {
        IWorkloadRepository _repository;

        public WorkloadController(IWorkloadRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("listworkloadwithfilter")]
        public IEnumerable<WorkloadsByUserViewModel> ListWorkloadWithFilter([FromQuery]string uniqueName)
        {
            return _repository.GetWorkloadsByUser(uniqueName);
        }

        [HttpGet]
        [Route("listworkloadbyuser")]
        public IEnumerable<WorkloadsByUserViewModel> ListWorkloadByUser()
        {
            var uniqueName = HttpContext.Request.Headers["unique_name"].ToString();
            var workloads = _repository.GetWorkloadsByUser(uniqueName);

            return workloads;
        }

        [HttpGet]
        [Route("list")]
        public IEnumerable<WorkloadViewModel> List()
        {
            var workloads = _repository.GetAllWorkloads();

            return workloads;
        }

        [HttpGet]
        [Route("details")]
        public WorkloadViewModel Details([FromQuery]Guid workloadID)
        {
            var workload = _repository.GetWorkloadByID(workloadID);
            return workload;
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Add()
        {
            var uniqueName = HttpContext.Request.Headers["unique_name"].ToString();

            System.IO.StreamReader reader = new System.IO.StreamReader(HttpContext.Request.Body);
            string requestFromPost = reader.ReadToEnd();
            var workload = JsonConvert.DeserializeObject<WorkloadViewModel>(requestFromPost);

            // Calling add
            var response = _repository.AddNewWorkload(workload);

            if (response)
            {
                _repository.SendNotificationAboutNewOrUpdatedWorkload(uniqueName, 0);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("updatestatus")]
        public HttpResponseMessage UpdateStatus([FromQuery]string id, [FromQuery]int status)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(HttpContext.Request.Body);
            string requestFromPost = reader.ReadToEnd();
            var statusUpdate = JsonConvert.DeserializeObject(requestFromPost);

            var response = _repository.UpdateWorkloadStatus(Guid.Parse(id), status);

            if (response)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("edit")]
        public HttpResponseMessage Edit()
        {
            var uniqueName = HttpContext.Request.Headers["unique_name"].ToString();

            System.IO.StreamReader reader = new System.IO.StreamReader(HttpContext.Request.Body);
            string requestFromPost = reader.ReadToEnd();
            var workload = JsonConvert.DeserializeObject<WorkloadViewModel>(requestFromPost);

            // Calling update
            var response = _repository.EditWorkload(workload);

            if (response)
            {
                _repository.SendNotificationAboutNewOrUpdatedWorkload(uniqueName, 1);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [Route("delete")]
        public HttpResponseMessage Delete([FromQuery]Guid workloadID)
        {
            var response = _repository.DeleteWorkloadByID(workloadID);
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
