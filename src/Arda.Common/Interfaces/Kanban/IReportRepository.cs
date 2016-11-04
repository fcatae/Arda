using Arda.Common.ViewModels.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arda.Common.Interfaces.Kanban
{
    public interface IReportRepository
    {
        IEnumerable<ActivityConsumingViewModel> GetActivityConsumingData(DateTime startDate, DateTime endDate, string user = "All");

        IEnumerable<ExpertiseConsumingViewModel> GetExpertiseConsumingData(DateTime startDate, DateTime endDate, string user = "All");

        IEnumerable<MetricConsumingViewModel> GetMetricConsumingData(DateTime startDate, DateTime endDate, string user = "All");
    }
}
