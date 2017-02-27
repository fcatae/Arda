using System;
using System.Collections.Generic;
using Arda.Common.ViewModels.Reports;

namespace Arda.Kanban.Models.Repositories
{
    public interface IReportRepository
    {
        IEnumerable<ActivityConsumingViewModel> GetActivityConsumingData(DateTime startDate, DateTime endDate, string user = "All");

        IEnumerable<ExpertiseConsumingViewModel> GetExpertiseConsumingData(DateTime startDate, DateTime endDate, string user = "All");

        IEnumerable<MetricConsumingViewModel> GetMetricConsumingData(DateTime startDate, DateTime endDate, string user = "All");
    }
}
