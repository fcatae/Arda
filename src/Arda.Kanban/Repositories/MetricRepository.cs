using Arda.Common.Interfaces.Kanban;
using System;
using System.Collections.Generic;
using System.Linq;
using Arda.Common.Models.Kanban;
using Arda.Common.ViewModels.Main;
using Arda.Kanban.Models;

namespace Arda.Kanban.Repositories
{
    public class MetricRepository : IMetricRepository
    {
        KanbanContext _context;

        public MetricRepository(KanbanContext context)
        {
            _context = context;
        }

        // Adds a new metric to the system.
        public bool AddNewMetric(MetricViewModel metric)
        {
            var fy = _context.FiscalYears.FirstOrDefault(f => f.FiscalYearID == metric.FiscalYearID);

            var metricToBeSaved = new Metric()
            {
                MetricID = metric.MetricID,
                MetricCategory = metric.MetricCategory,
                MetricName = metric.MetricName,
                Description = metric.Description,
                FiscalYear = fy
            };

            _context.Metrics.Add(metricToBeSaved);
            var response = _context.SaveChanges();

            if (response > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Return all metrics
        //TODO: Remove Nullable Casting
        public List<MetricViewModel> GetAllMetrics()
        {
            var response = (from m in _context.Metrics
                            join f in _context.FiscalYears on m.FiscalYear.FiscalYearID equals f.FiscalYearID
                            orderby f.FullNumericFiscalYear, m.MetricCategory
                            select new MetricViewModel
                            {
                                MetricID = m.MetricID,
                                MetricCategory = m.MetricCategory,
                                MetricName = m.MetricName,
                                Description = m.Description,
                                FiscalYearID = m.FiscalYear.FiscalYearID,
                                FullNumericFiscalYear = m.FiscalYear.FullNumericFiscalYear,
                                TextualFiscalYear = m.FiscalYear.TextualFiscalYear
                            }).ToList();

            return response;
        }

        // Return all metrics by year
        //TODO: Remove Nullable Casting
        public List<MetricViewModel> GetAllMetrics(int year)
        {
            var response = (from m in _context.Metrics
                            join f in _context.FiscalYears on m.FiscalYear.FiscalYearID equals f.FiscalYearID
                            where f.FullNumericFiscalYear == year
                            orderby f.FullNumericFiscalYear, m.MetricCategory
                            select new MetricViewModel
                            {
                                MetricID = m.MetricID,
                                MetricCategory = m.MetricCategory,
                                MetricName = m.MetricName,
                                Description = m.Description,
                                FiscalYearID = m.FiscalYear.FiscalYearID,
                                FullNumericFiscalYear = m.FiscalYear.FullNumericFiscalYear,
                                TextualFiscalYear = m.FiscalYear.TextualFiscalYear
                            }).ToList();

            return response;
        }

        // Return metric based on ID
        public MetricViewModel GetMetricByID(Guid id)
        {
            var metric = (from m in _context.Metrics
                          join f in _context.FiscalYears on m.FiscalYear.FiscalYearID equals f.FiscalYearID
                          where m.MetricID == id
                          select new MetricViewModel
                          {
                              MetricID = m.MetricID,
                              MetricCategory = m.MetricCategory,
                              MetricName = m.MetricName,
                              Description = m.Description,
                              FiscalYearID = (Guid)m.FiscalYear.FiscalYearID,
                              FullNumericFiscalYear = (int)m.FiscalYear.FullNumericFiscalYear,
                              TextualFiscalYear = (string)m.FiscalYear.TextualFiscalYear
                          }).First();

            return metric;
        }

        // Update metric data based on ID
        public bool EditMetricByID(MetricViewModel metric)
        {
            var metricToBeUpdated = (from m in _context.Metrics
                                     where m.MetricID == metric.MetricID
                                     select m).First();

            var fyToBeIncluded = (from fy in _context.FiscalYears
                                  where fy.FiscalYearID == metric.FiscalYearID
                                  select fy).First();


            if (metricToBeUpdated != null)
            {
                // Update informations of object
                metricToBeUpdated.MetricName = metric.MetricName;
                metricToBeUpdated.MetricCategory = metric.MetricCategory;
                metricToBeUpdated.Description = metric.Description;
                metricToBeUpdated.FiscalYear = fyToBeIncluded;

                var response = _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        // Delete metric based on ID
        public bool DeleteMetricByID(Guid id)
        {
            var metricToBeDeleted = _context.Metrics.First(m => m.MetricID == id);

            if (metricToBeDeleted != null)
            {
                var response = _context.Remove(metricToBeDeleted);
                _context.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
