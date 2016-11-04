using Arda.Common.Interfaces.Kanban;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arda.Common.ViewModels.Reports;
using Arda.Kanban.Models;
using Arda.Common.Models.Kanban;
using Arda.Common.Utils;

namespace Arda.Kanban.Repositories
{
    public class ReportRepository : IReportRepository
    {
        KanbanContext _context;

        public ReportRepository(KanbanContext context)
        {
            _context = context;
        }

        public IEnumerable<ActivityConsumingViewModel> GetActivityConsumingData(DateTime startDate, DateTime endDate, string user = "All")
        {
            try
            {
                List<ActivityConsumingViewModel> activities;

                if (user == "All")
                {
                    activities = (from ap in _context.Appointments
                                  join w in _context.WorkloadBacklogs on ap.AppointmentWorkload.WBID equals w.WBID
                                  join a in _context.Activities on w.WBActivity.ActivityID equals a.ActivityID
                                  where ap.AppointmentDate >= startDate && ap.AppointmentDate <= endDate
                                  select new ActivityConsumingViewModel()
                                  {
                                      Activity = a.ActivityName,
                                      Hours = ap.AppointmentHoursDispensed
                                  }).ToList();
                }
                else
                {
                    activities = (from ap in _context.Appointments
                                  join w in _context.WorkloadBacklogs on ap.AppointmentWorkload.WBID equals w.WBID
                                  join a in _context.Activities on w.WBActivity.ActivityID equals a.ActivityID
                                  where ap.AppointmentDate >= startDate && ap.AppointmentDate <= endDate && ap.AppointmentUser.UniqueName == user
                                  select new ActivityConsumingViewModel()
                                  {
                                      Activity = a.ActivityName,
                                      Hours = ap.AppointmentHoursDispensed
                                  }).ToList();
                }
                var totalHours = (Convert.ToDecimal(activities.Sum(a => a.Hours))) / 100;

                var activityConsuming = activities
                     .GroupBy(a => a.Activity)
                     .Select(ac => new ActivityConsumingViewModel
                     {
                         Activity = ac.Key,
                         Hours = ac.Sum(a => a.Hours),
                         Percent = Math.Round(ac.Sum(a => a.Hours) / totalHours, 2)
                     })
                     .OrderByDescending(ac => ac.Hours)
                     .ToList();

                return activityConsuming;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<ExpertiseConsumingViewModel> GetExpertiseConsumingData(DateTime startDate, DateTime endDate, string user = "All")
        {
            try
            {
                List<ExpertiseConsumingViewModel> expertises;

                if (user == "All")
                {
                    expertises = (from ap in _context.Appointments
                                  join w in _context.WorkloadBacklogs on ap.AppointmentWorkload.WBID equals w.WBID
                                  where ap.AppointmentDate >= startDate && ap.AppointmentDate <= endDate
                                  select new ExpertiseConsumingViewModel()
                                  {
                                      Expertise = Extensions.EnumHelper<Expertise>.GetDisplayValue(w.WBExpertise),
                                      Hours = ap.AppointmentHoursDispensed
                                  }).ToList();
                }
                else
                {
                    expertises = (from ap in _context.Appointments
                                  join w in _context.WorkloadBacklogs on ap.AppointmentWorkload.WBID equals w.WBID
                                  where ap.AppointmentDate >= startDate && ap.AppointmentDate <= endDate && ap.AppointmentUser.UniqueName == user
                                  select new ExpertiseConsumingViewModel()
                                  {
                                      Expertise = Extensions.EnumHelper<Expertise>.GetDisplayValue(w.WBExpertise),
                                      Hours = ap.AppointmentHoursDispensed
                                  }).ToList();
                }
                var totalHours = (Convert.ToDecimal(expertises.Sum(a => a.Hours))) / 100;

                var categoryConsuming = expertises
                     .GroupBy(c => c.Expertise)
                     .Select(cc => new ExpertiseConsumingViewModel
                     {
                         Expertise = cc.Key,
                         Hours = cc.Sum(a => a.Hours),
                         Percent = Math.Round(cc.Sum(a => a.Hours) / totalHours, 2)
                     })
                     .OrderByDescending(ac => ac.Hours)
                     .ToList();

                return categoryConsuming;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<MetricConsumingViewModel> GetMetricConsumingData(DateTime startDate, DateTime endDate, string user = "All")
        {
            try
            {
                List<MetricConsumingViewModel> metrics;

                if (user == "All")
                {
                    metrics = (from ap in _context.Appointments
                               join w in _context.WorkloadBacklogs on ap.AppointmentWorkload.WBID equals w.WBID
                               join wbm in _context.WorkloadBacklogMetrics on w.WBID equals wbm.WorkloadBacklog.WBID
                               join m in _context.Metrics on wbm.Metric.MetricID equals m.MetricID
                               where ap.AppointmentDate >= startDate && ap.AppointmentDate <= endDate
                               select new MetricConsumingViewModel
                               {
                                   Metric = m.MetricName,
                                   Hours = ap.AppointmentHoursDispensed
                               }).ToList();
                }
                else
                {
                    metrics = (from ap in _context.Appointments
                               join w in _context.WorkloadBacklogs on ap.AppointmentWorkload.WBID equals w.WBID
                               join wbm in _context.WorkloadBacklogMetrics on w.WBID equals wbm.WorkloadBacklog.WBID
                               join m in _context.Metrics on wbm.Metric.MetricID equals m.MetricID
                               where ap.AppointmentDate >= startDate && ap.AppointmentDate <= endDate && ap.AppointmentUser.UniqueName == user
                               select new MetricConsumingViewModel
                               {
                                   Metric = m.MetricName,
                                   Hours = ap.AppointmentHoursDispensed
                               }).ToList();

                }
                var totalHours = (Convert.ToDecimal(metrics.Sum(a => a.Hours))) / 100;

                var metricConsuming = metrics
                     .GroupBy(m => m.Metric)
                     .Select(ac => new MetricConsumingViewModel
                     {
                         Metric = ac.Key,
                         Hours = ac.Sum(a => a.Hours),
                         //Percent = Math.Round(ac.Sum(a => a.Hours) / totalHours, 2)
                     })
                     .OrderByDescending(ac => ac.Hours)
                     .ToList();

                return metricConsuming;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
