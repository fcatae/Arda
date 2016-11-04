using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Arda.Common.Interfaces.Kanban;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using Arda.Common.ViewModels.Main;

namespace Arda.Kanban.Controllers
{
    [Route("api/[controller]")]
    public class MetricController : Controller
    {
        IMetricRepository _repository;

        public MetricController(IMetricRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Add()
        {
            try
            {
                System.IO.StreamReader reader = new System.IO.StreamReader(HttpContext.Request.Body);
                string requestFromPost = reader.ReadToEnd();
                var metric = JsonConvert.DeserializeObject<MetricViewModel>(requestFromPost);

                // Calling update
                var response = _repository.AddNewMetric(metric);

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

        [HttpGet]
        [Route("list")]
        public IEnumerable<MetricViewModel> List()
        {
            try
            {
                var metrics = _repository.GetAllMetrics();

                if (metrics != null)
                {
                    return metrics;
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
        [Route("listbyyear")]
        public IEnumerable<MetricViewModel> List(int year)
        {
            try
            {
                var metrics = _repository.GetAllMetrics(year);

                if (metrics != null)
                {
                    return metrics;
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
        [Route("getmetricbyid")]
        public MetricViewModel GetMetricByID(Guid id)
        {
            try
            {
                var metric = _repository.GetMetricByID(id);

                if (metric != null)
                {
                    return metric;
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
        [Route("editmetricbyid")]
        public HttpResponseMessage EditMetricByID()
        {
            try
            {
                System.IO.StreamReader reader = new System.IO.StreamReader(HttpContext.Request.Body);
                string requestFromPost = reader.ReadToEnd();
                var metric = JsonConvert.DeserializeObject<MetricViewModel>(requestFromPost);

                // Calling update
                var response = _repository.EditMetricByID(metric);

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

        [HttpDelete]
        [Route("deletemetricbyid")]
        public HttpResponseMessage DeleteMetricByID(Guid id)
        {
            try
            {
                var response = _repository.DeleteMetricByID(id);

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
