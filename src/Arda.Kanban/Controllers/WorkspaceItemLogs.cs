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
        public WorkspaceItem GetLogs(Guid itemId)
        {
            if (itemId == Guid.Empty)
                throw new ArgumentNullException("itemId is empty");

            // return a list of appointments
            
            return null;
        }
                
        [HttpPost("{itemId}/logs")]
        [ProducesResponseType(202)]
        public IActionResult AppendLog(Guid itemId, string text, [FromQuery]string user)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest("newItem is invalid");

            // add the details

            return NoContent();
        }
    }
}
