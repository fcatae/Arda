using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Arda.Kanban.Models.Repositories;
using Arda.Kanban.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Arda.Kanban.Controllers
{
    [Route("api/[controller]")]
    public class TechnologyController : Controller
    {
        ITechnologyRepository _repository;

        public TechnologyController(ITechnologyRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("list")]
        public IEnumerable<TechnologyViewModel> List()
        {
            var technologies = _repository.GetAllTechnologies();

            return technologies;
        }
    }
}
