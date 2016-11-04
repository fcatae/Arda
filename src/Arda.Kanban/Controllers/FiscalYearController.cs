using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Arda.Common.Models.Kanban;
using Arda.Common.Interfaces.Kanban;
using Arda.Common.ViewModels.Main;
using Newtonsoft.Json;
using System.Net.Http;

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
            try
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
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("list")]
        public IEnumerable<FiscalYearViewModel> List()
        {
            try
            {
                var fiscalyears = _repository.GetAllFiscalYears();

                if (fiscalyears != null)
                {
                    return fiscalyears;
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

        [HttpGet]
        [Route("getfiscalyearbyid")]
        public FiscalYearViewModel GetFiscalYearByID(Guid id)
        {
            try
            {
                var fiscalYear = _repository.GetFiscalYearByID(id);

                if (fiscalYear != null)
                {
                    return fiscalYear;
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

        [HttpPut]
        [Route("editfiscalyearbyid")]
        public HttpResponseMessage EditFiscalYearByID()
        {
            try
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
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [Route("deletefiscalyearbyid")]
        public HttpResponseMessage DeleteFiscalYearByID(Guid id)
        {
            try
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
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
