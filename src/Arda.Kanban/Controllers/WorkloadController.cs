using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using Arda.Kanban.Models.Repositories;
using Arda.Kanban.ViewModels;

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
        // replace with /folders/username@domain.com
        // replace with /workspace/default/folders/dashboard (Auth)
        public IEnumerable<WorkloadsByUserViewModel> ListWorkloadWithFilter([FromQuery]string uniqueName)
        {
            return _repository.GetWorkloadsByUser(uniqueName);
        }

        [HttpGet]
        [Route("listworkloadbyuser")] 
        // DEPRECATED: Using headers for determining the user
        public IEnumerable<WorkloadsByUserViewModel> ListWorkloadByUser()
        {
            var uniqueName = HttpContext.Request.Headers["unique_name"].ToString();
            var workloads = _repository.GetWorkloadsByUser(uniqueName);

            return workloads;
        }

        [HttpGet]
        [Route("list")] 
        // DEPRECATED: No list all items
        public IEnumerable<WorkloadViewModel> List()
        {
            var workloads = _repository.GetAllWorkloads();

            return workloads;
        }

        [HttpGet]
        [Route("details")]
        // replace with /items/<guid>
        // replace with /workspace/<workload>/items/<guid>
        public WorkloadViewModel Details([FromQuery]Guid workloadID)
        {
            var workload = _repository.GetWorkloadByID(workloadID);
            return workload;
        }

        [HttpPost]
        [Route("add")]
        // replace with POST /folders/user@domain.com 
        // replace with POST /workspace/default/folders/dashboard (Auth)
        public HttpResponseMessage Add()
        {
            // var uniqueName = HttpContext.Request.Headers["unique_name"].ToString();

            System.IO.StreamReader reader = new System.IO.StreamReader(HttpContext.Request.Body);
            string requestFromPost = reader.ReadToEnd();
            var workload = JsonConvert.DeserializeObject<WorkloadViewModel>(requestFromPost);

            // Calling add
            var response = _repository.AddNewWorkload(workload);
            // _repository.EditWorkload(workload); // try to reproduce bug #44

            if (response)
            {
                //_repository.SendNotificationAboutNewOrUpdatedWorkload(uniqueName, 0);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("updatestatus")] // replace with PUT /workspace/default/items/<guid>/status
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
        [Route("edit")] // replace with PUT /workspace/default/items/<guid>/status
        public HttpResponseMessage Edit()
        {
            // var uniqueName = HttpContext.Request.Headers["unique_name"].ToString();

            System.IO.StreamReader reader = new System.IO.StreamReader(HttpContext.Request.Body);
            string requestFromPost = reader.ReadToEnd();
            var workload = JsonConvert.DeserializeObject<WorkloadViewModel>(requestFromPost);

            // Calling update
            var response = _repository.EditWorkload(workload);

            if (response)
            {
                // _repository.SendNotificationAboutNewOrUpdatedWorkload(uniqueName, 1);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [Route("delete")] // replace with DELETE /workspace/default/items/<guid>
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

        [HttpGet] // replace with /workspace/default/folders/archive
        [Route("listarchivewithfilter")]
        public IEnumerable<WorkloadsByUserViewModel> ListArchiveWithFilter([FromQuery]string uniqueName)
        {
            return _repository.GetArchivedWorkloadsByUser(uniqueName);
        }

        [HttpGet] // replace with /workspace/default/folders/default/search
        [Route("search")]
        public IEnumerable<WorkloadsByUserViewModel> SimpleSearchWithFilter([FromQuery]string uniqueName, [FromQuery]string searchText)
        {
            return _repository.GetWorkloadsByUser(uniqueName);
        }
        
    }
}
