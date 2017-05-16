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
    public class WorkspaceItemLogsController : Controller
    {
        Repositories.WorkspaceRepository _repository;
        
        public WorkspaceItemLogsController(IWorkspaceRepository repository)
        {
            _repository = (Repositories.WorkspaceRepository)repository;
        }
        
        [HttpGet("{itemId}/logs")]
        public IEnumerable<WorkspaceItemLog> GetLogs(Guid itemId)
        {
            if (itemId == Guid.Empty)
                throw new ArgumentNullException("itemId is empty");

            var logs = _repository.GetLogs(itemId);
            
            return logs;
        }
                
        public class InputAppendLog
        {
            public string Text { get; set; }
            public DateTime? CreatedDate { get; set; }
            public WorkspaceItemLogProperties Property { get; set; }
        }

        [HttpPost("{itemId}/logs")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AppendLog(Guid itemId, [FromBody]InputAppendLog input, [FromQuery]string user)
        {
            if (!ModelState.IsValid)
                return BadRequest("newItem is invalid");

            if (input.Text == null)
                return BadRequest("input.Text == null");

            if (user == null)
                return BadRequest("user == null");

            // add the details
            var log = new WorkspaceItemLog()
            {
                Id = Guid.NewGuid(),
                CreatedBy = user,
                CreatedDate = input.CreatedDate ?? DateTime.Now,
                Text = input.Text
            };

            _repository.AppendLog(itemId, log);

            return NoContent();
        }
    }
}
