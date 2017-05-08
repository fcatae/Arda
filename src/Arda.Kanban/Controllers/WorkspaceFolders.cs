using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using System.Linq;
using Swashbuckle.AspNetCore.SwaggerGen;

using Arda.Kanban.Models;
using Arda.Kanban.Models.Repositories;
using Arda.Kanban.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Arda.Kanban.Controllers
{
    // default = authenticated user

    [Route("v2/folders")]
    public class WorkspaceFoldersController : Controller
    {
        Arda.Kanban.Repositories.WorkspaceRepository _repository;
        
        public WorkspaceFoldersController(IWorkspaceRepository repository)
        {
            _repository = (Arda.Kanban.Repositories.WorkspaceRepository)repository;
        }

        [HttpGet("{folderId}")]
        public IEnumerable<WorkspaceItem> ListItems(string folderId, [FromQuery]bool? archived)
        {
            if( archived == true )
                return _repository.ListArchivedByUser(folderId);

            return _repository.ListByUser(folderId);
        }

        public class AddItemInput
        {
            [Required]
            public string Title { get; set; }
            // optional
            public Guid? Id { get; set; }
            public string Summary { get; set; }
            public string Description { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public int? ItemState { get; set; }
        }

        [HttpPost("{folderId}/add")]
        public IActionResult AddItem(string folderId, [FromBody]AddItemInput workloadInput)
        {
            if (folderId == null || folderId == "")
                return BadRequest("folderId is empty");

            if (!ModelState.IsValid)
                return BadRequest("workloadInput is invalid");

            var workload = new WorkspaceItem()
            {
                // must exist
                Title = workloadInput.Title,

                // optional
                Id = (workloadInput.Id == null) ? Guid.NewGuid() : workloadInput.Id.Value,
                Summary = workloadInput.Summary,
                Description = workloadInput.Description,
                StartDate = workloadInput.StartDate,
                EndDate = workloadInput.EndDate,

                ItemState = (workloadInput.ItemState == null) ? 0 : workloadInput.ItemState.Value,

                // auto-created
                CreatedBy = folderId,
                CreatedDate = DateTime.Now
            };

            _repository.Create(workload);

            return CreatedAtRoute("GetItem", new { itemId = workload.Id }, workload);
        }
    }
}
