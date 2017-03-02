using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arda.Kanban.Models
{
    [Table("Metrics")]
    public class Metric
    {
        [Key]
        [Required]
        public Guid MetricID { get; set; }

        [Required]
        public string MetricCategory { get; set; }

        [Required]
        public string MetricName { get; set; }

        [Required]
        public string Description { get; set; }

        public Guid FiscalYearID { get; set; }

        [ForeignKey("FiscalYearID")]
        public virtual FiscalYear FiscalYear { get; set; }

        public virtual ICollection<WorkloadBacklogMetric> WBMetrics { get; set; }
    }
}
