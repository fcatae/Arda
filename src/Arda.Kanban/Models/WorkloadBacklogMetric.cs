using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arda.Kanban.Models
{
    [Table("WorkloadBacklogMetrics")]
    public class WorkloadBacklogMetric
    {
        [Key]
        [Required]
        public Guid WBMetricID { get; set; }

        public Guid WorkloadBacklogWBID { get; set; }

        [ForeignKey("WorkloadBacklogWBID")]
        public virtual WorkloadBacklog WorkloadBacklog { get; set; }

        public Guid MetricMetricID { get; set; }

        [ForeignKey("MetricMetricID")]
        public virtual Metric Metric { get; set; }
    }
}
