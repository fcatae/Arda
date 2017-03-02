using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Arda.Common.ViewModels.Main;
using Arda.Kanban.Models.Repositories;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Arda.Kanban.Controllers
{
    [Route("api/[controller]")]
    public class ActivityController : Controller
    {
        IActivityRepository _repository;

        public ActivityController(IActivityRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("list")]
        public IEnumerable<ActivityViewModel> List()
        {
            var activities = _repository.GetAllActivities();

            return activities;
        }
    }
}
