using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http;
using Arda.Kanban.Models.Repositories;
using Arda.Kanban.ViewModels;

namespace Arda.Kanban.Controllers
{
    [Route("api/[controller]")]
    public class FiscalYearController : Controller
    {
        private IFiscalYearRepository _repository;

        public FiscalYearController(IFiscalYearRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Route("addfiscalyear")]
        public HttpResponseMessage Add()
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(HttpContext.Request.Body);
            string requestFromPost = reader.ReadToEnd();
            var fiscalYear = JsonConvert.DeserializeObject<FiscalYearViewModel>(requestFromPost);

            // Calling update
            var fiscalyear = _repository.AddNewFiscalYear(fiscalYear);

            if (fiscalyear)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("list")]
        public IEnumerable<FiscalYearViewModel> List()
        {
            var fiscalyears = _repository.GetAllFiscalYears();

            return fiscalyears;
        }

        [HttpGet]
        [Route("getfiscalyearbyid")]
        public FiscalYearViewModel GetFiscalYearByID([FromQuery]Guid id)
        {
            var fiscalYear = _repository.GetFiscalYearByID(id);

            return fiscalYear;
        }

        [HttpPut]
        [Route("editfiscalyearbyid")]
        public HttpResponseMessage EditFiscalYearByID()
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(HttpContext.Request.Body);
            string requestFromPost = reader.ReadToEnd();
            var fiscalYear = JsonConvert.DeserializeObject<FiscalYearViewModel>(requestFromPost);

            // Calling update
            var fiscalyear = _repository.EditFiscalYearByID(fiscalYear);

            if (fiscalyear)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [Route("deletefiscalyearbyid")]
        public HttpResponseMessage DeleteFiscalYearByID([FromQuery]Guid id)
        {
            var response = _repository.DeleteFiscalYearByID(id);

            if (response)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
