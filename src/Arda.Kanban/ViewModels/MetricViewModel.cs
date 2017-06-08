using System;

namespace Arda.Kanban.ViewModels
{
    public class MetricViewModel
    {
        //Metric
        public Guid MetricID { get; set; }

        public string MetricCategory { get; set; }

        public string MetricName { get; set; }

        public string Description { get; set; }

        //FY Foreign Key:
        public Guid FiscalYearID { get; set; }

        public int FullNumericFiscalYear { get; set; }

        public string TextualFiscalYear { get; set; }
    }
}
