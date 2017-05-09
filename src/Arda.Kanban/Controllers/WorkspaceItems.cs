using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using Arda.Kanban.Models.Repositories;
using Arda.Kanban.ViewModels;
using System.Linq;
using Arda.Kanban.Models;

namespace Arda.Kanban.Controllers
{
    // default = authenticated user

    [Route("v2/items")]
    public class WorkspaceItemsController : Controller
    {
        Repositories.WorkspaceRepository _repository;
        
        public WorkspaceItemsController(IWorkspaceRepository repository)
        {
            _repository = (Repositories.WorkspaceRepository)repository;
        }
        
        [HttpGet("{itemId}", Name="GetItem")]
        public WorkspaceItem GetItem(Guid itemId)
        {
            if (itemId == Guid.Empty)
                throw new ArgumentNullException("itemId is empty");

            WorkspaceItem workload = _repository.TryGet(itemId);

            if (workload == null)
                throw new InvalidOperationException("No object found for itemId");

            return workload;
        }

        [HttpPut("{itemId}")]
        public IActionResult Edit(Guid itemId, [FromBody]WorkspaceItem newItem)
        {
            if (itemId == Guid.Empty)
                throw new ArgumentNullException("itemId is empty");

            if (!ModelState.IsValid)
                return BadRequest("newItem is invalid");

            // What if the item does not exist?
            
            // WorkspaceItem workload = _repository.TryGet(itemId);

            _repository.Upsert(newItem);
            
            return Accepted();
        }

        [HttpDelete("{itemId}")]
        public IActionResult Delete(Guid itemId)
        {
            _repository.Delete(itemId);

            return NoContent();
        }

        [HttpPut("{itemId}/status/{newStatus}")]
        public IActionResult UpdateStatus(Guid itemId, int newStatus)
        {
            if (!ModelState.IsValid)
                return BadRequest("newItem is invalid");

            _repository.SetStatus(itemId, newStatus);

            return Accepted();
        }
    }
}
