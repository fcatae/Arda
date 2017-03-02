using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using Arda.Kanban.Models.Repositories;
using Arda.Kanban.ViewModels;

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

        [HttpGet]
        [Route("list")]
        public IEnumerable<MetricViewModel> List()
        {
            var metrics = _repository.GetAllMetrics();

            return metrics;
        }

        [HttpGet]
        [Route("listbyyear")]
        public IEnumerable<MetricViewModel> List([FromQuery]int year)
        {
            var metrics = _repository.GetAllMetrics(year);

            return metrics;
        }

        [HttpGet]
        [Route("getmetricbyid")]
        public MetricViewModel GetMetricByID([FromQuery]Guid id)
        {
            var metric = _repository.GetMetricByID(id);

            return metric;
        }

        [HttpPut]
        [Route("editmetricbyid")]
        public HttpResponseMessage EditMetricByID()
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

        [HttpDelete]
        [Route("deletemetricbyid")]
        public HttpResponseMessage DeleteMetricByID([FromQuery]Guid id)
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
    }
}
