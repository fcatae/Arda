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

    [Route("v2/folders")]
    public class WorkspaceFoldersController : Controller
    {
        IWorkloadRepository _repository;
        
        public WorkspaceFoldersController(IWorkloadRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{folderId}")]
        public object GetItems(string folderId)
        {
            object ret = _repository.GetWorkloadsByUser(folderId);

            return ret;
        }

        [HttpPost("{folderId}")]
        public IActionResult Create(string folderId, [FromBody]string input)
        {
            //object ret = _repository.GetWorkloadsByUser(folderId);

            return CreatedAtRoute("GetItem", new { itemId = 1 }, input);
        }

    }
}
