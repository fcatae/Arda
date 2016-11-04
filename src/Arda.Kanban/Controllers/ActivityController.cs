using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Arda.Common.Interfaces.Kanban;
using Arda.Common.ViewModels.Main;

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
            try
            {
                var activities = _repository.GetAllActivities();

                if (activities != null)
                {
                    return activities;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
