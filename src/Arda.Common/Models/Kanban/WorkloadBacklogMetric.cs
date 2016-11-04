using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Arda.Common.Models.Kanban
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
