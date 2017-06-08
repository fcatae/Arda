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
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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

        // Upsert can create orphaned objects -- weird behavior!
        [HttpPut("")]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult Upsert([FromBody, Required]WorkspaceItem newItem)
        {
            if (newItem.Id == Guid.Empty)
                return BadRequest("itemId is empty");

            if (!ModelState.IsValid)
                return BadRequest("newItem is invalid");

            bool itemExists = _repository.Edit(newItem);

            if (!itemExists)
            {
                _repository.Create(newItem);
            }

            return Accepted();
        }

        [HttpPost("")]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult Edit([FromBody, Required]WorkspaceItem newItem)
        {
            if (newItem.Id == Guid.Empty)
                return BadRequest("itemId is empty");

            if (!ModelState.IsValid)
                return BadRequest("newItem is invalid");

            bool mustExists = _repository.Edit(newItem);

            if (!mustExists)
                return BadRequest("item does not exist");

            return Accepted();
        }

        public class EditItemInput
        {
            public string Title { get; set; }
            public string Summary { get; set; }
            public string Description { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public int? ItemState { get; set; }
        }

        [HttpPatch("{itemId}")]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult EditItem(Guid itemId, [FromBody, Required]EditItemInput editItem)
        {
            if (itemId == Guid.Empty)
                return BadRequest("itemId is empty");
            
            var workload = _repository.Get(itemId);

            if (workload == null)
                return BadRequest("item does not exist");

            bool sameWorkload = itemId == workload.Id;
            if (!sameWorkload)
                throw new InvalidOperationException("not the same Workload");

            var newItem = new WorkspaceItem()
            {
                Id = itemId,

                // first [editItem], then [workload]
                Title       = editItem.Title ?? workload.Title,
                Description = editItem.Description ?? workload.Description,
                EndDate     = editItem.EndDate ?? workload.EndDate,
                ItemState   = editItem.ItemState ?? workload.ItemState,
                StartDate   = editItem.StartDate ?? workload.StartDate,
                Summary     = editItem.Summary ?? workload.Summary,

                // values that cannot be changed
                CreatedBy = null,
                CreatedDate = default(DateTime)
            };

            bool assertAlwaysExists = _repository.Edit(newItem);

            if (!assertAlwaysExists)
                throw new InvalidOperationException("workload not found for editing");

            return Accepted();
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
        
        [HttpDelete("{itemId}")]
        [ProducesResponseType(204)]
        public IActionResult Delete(Guid itemId)
        {
            _repository.Delete(itemId);

            return NoContent();
        }

        [HttpPut("{itemId}/status/{newStatus}")]
        [ProducesResponseType(202)]
        public IActionResult UpdateStatus(Guid itemId, int newStatus)
        {
            if (!ModelState.IsValid)
                return BadRequest("newItem is invalid");

            _repository.SetStatus(itemId, newStatus);

            return Accepted();
        }
    }
}
