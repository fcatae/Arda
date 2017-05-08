using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using Arda.Kanban.Models.Repositories;
using Arda.Kanban.ViewModels;
using System.Linq;

namespace Arda.Kanban.Controllers
{
    // default = authenticated user

    [Route("v2/items")]
    public class WorkspaceItemsController : Controller
    {
        IWorkloadRepository _repository;
        
        public WorkspaceItemsController(IWorkloadRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet("{itemId}", Name="GetItem")]
        public object GetItem(Guid itemId)
        {
            object ret = _repository.GetWorkloadByID(itemId);

            return ret;
        }

        [HttpPut("{itemId}")]
        public IActionResult Edit(Guid itemId, [FromBody]object newItem)
        {
            // should replace the entire object
            return Accepted();
        }

        [HttpDelete("{itemId}")]
        public IActionResult Delete(Guid itemId, [FromBody]object newItem)
        {
            // delete the resource            
            return NoContent(); //Accepted();
        }

        [HttpPut("{itemId}/status/{newStatus}")]
        public IActionResult UpdateStatus(Guid itemId, int newStatus)
        {
            return Accepted();
        }
    }
}
