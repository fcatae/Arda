using System;
using System.Collections.Generic;
using System.Linq;
using Arda.Kanban.Models;
using Arda.Kanban.Models.Repositories;
using Arda.Kanban.ViewModels;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Arda.Kanban.MongoRepositories
{
    public class MetricMongoRepository : IMetricRepository
    {
        FiscalYearMongoRepository fiscalYearRepository;
        MongoContext _context;
        BsonDocument all = new BsonDocument() { };
        BsonDocument byFiscalYearMetricCategory = new BsonDocument() {
            { nameof(Metric) + "." + nameof(FiscalYear._id), 1 },
            { nameof(Metric.metricCategory), 1 } };

        // FiscalYearID, m.MetricCategory
        public MetricMongoRepository(MongoContext context)
        {
            fiscalYearRepository = new FiscalYearMongoRepository(context);
            _context = context;            
        }

        // Adds a new metric to the system.
        public bool AddNewMetric(MetricViewModel metric)
        {
            var currentFiscalYear = metric.FiscalYearID;

            var fy = fiscalYearRepository.GetFiscalYearByID(currentFiscalYear);
            
            var model = new Metric()
            {
                _id = metric.MetricID.ToString(),
                metricCategory = metric.MetricCategory,
                metricName = metric.MetricName,
                description = metric.Description,
                FiscalYear = new FiscalYear()
                {
                    _id = fy.FiscalYearID.ToString(),
                    fullNumericFiscalYear = fy.FullNumericFiscalYearMain,
                    textualFiscalYear = fy.TextualFiscalYearMain
                }
            };

            _context.Metrics.InsertOne(model);

            return true;
        }
        
        public List<MetricViewModel> GetAllMetrics()
        {
            var response = (from m in _context.Metrics.Find(all).Sort(byFiscalYearMetricCategory).ToEnumerable()
                            select new MetricViewModel
                            {
                                MetricID = Guid.Parse(m._id),
                                MetricCategory = m.metricCategory,
                                MetricName = m.metricName,
                                Description = m.description,
                                FiscalYearID = Guid.Parse(m.FiscalYear._id),
                                FullNumericFiscalYear = m.FiscalYear.fullNumericFiscalYear,
                                TextualFiscalYear = m.FiscalYear.textualFiscalYear
                            }).ToList();

            return response;
        }

        // Return all metrics by year
        //TODO: Remove Nullable Casting
        public List<MetricViewModel> GetAllMetrics(int year)
        {
            //var response = (from m in _context.Metrics
            //                //join f in _context.FiscalYears on m.FiscalYear.FiscalYearID equals f.FiscalYearID
            //                where m.FiscalYear.FullNumericFiscalYear == year
            //                orderby m.FiscalYear.FullNumericFiscalYear, m.MetricCategory
            //                select new MetricViewModel
            //                {
            //                    MetricID = m.MetricID,
            //                    MetricCategory = m.MetricCategory,
            //                    MetricName = m.MetricName,
            //                    Description = m.Description,
            //                    FiscalYearID = m.FiscalYear.FiscalYearID,
            //                    FullNumericFiscalYear = m.FiscalYear.FullNumericFiscalYear,
            //                    TextualFiscalYear = m.FiscalYear.TextualFiscalYear
            //                }).ToList();

            //return response;
            throw new NotImplementedException();
        }

        // Return metric based on ID
        public MetricViewModel GetMetricByID(Guid id)
        {
            //var metric = (from m in _context.Metrics
            //              //join f in _context.FiscalYears on m.FiscalYear.FiscalYearID equals f.FiscalYearID
            //              where m.MetricID == id
            //              select new MetricViewModel
            //              {
            //                  MetricID = m.MetricID,
            //                  MetricCategory = m.MetricCategory,
            //                  MetricName = m.MetricName,
            //                  Description = m.Description,
            //                  FiscalYearID = (Guid)m.FiscalYear.FiscalYearID,
            //                  FullNumericFiscalYear = (int)m.FiscalYear.FullNumericFiscalYear,
            //                  TextualFiscalYear = (string)m.FiscalYear.TextualFiscalYear
            //              }).First();

            throw new NotImplementedException();
        }

        // Update metric data based on ID
        public bool EditMetricByID(MetricViewModel metric)
        {
            //var metricToBeUpdated = (from m in _context.Metrics
            //                         where m.MetricID == metric.MetricID
            //                         select m).First();

            //var fyToBeIncluded = (from fy in _context.FiscalYears
            //                      where fy.FiscalYearID == metric.FiscalYearID
            //                      select fy).First();


            //if (metricToBeUpdated != null)
            //{
            //    // Update informations of object
            //    metricToBeUpdated.MetricName = metric.MetricName;
            //    metricToBeUpdated.MetricCategory = metric.MetricCategory;
            //    metricToBeUpdated.Description = metric.Description;
            //    metricToBeUpdated.FiscalYear = fyToBeIncluded;

            //    var response = _context.SaveChanges();
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            throw new NotImplementedException();
        }

        // Delete metric based on ID
        public bool DeleteMetricByID(Guid id)
        {
            throw new NotImplementedException();

            //var metricToBeDeleted = _context.Metrics.First(m => m.MetricID == id);

            //if (metricToBeDeleted != null)
            //{
            //    var response = _context.Remove(metricToBeDeleted);
            //    _context.SaveChanges();

            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }
    }
}
