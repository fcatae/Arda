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
    [Route("v2/workspaces/{workspaceId}/folders/{folderId}")]
    public class WorkspaceFoldersController : Controller
    {
        IWorkloadRepository _repository;
        
        public WorkspaceFoldersController(IWorkloadRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet("")]
        public string Root(string workspaceId, string folderId)
        {
            return workspaceId + "/" + folderId;
        }
    }
}
