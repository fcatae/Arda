using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arda.Kanban.Models
{
    [Table("WorkloadBacklogTechnologies")]
    public class WorkloadBacklogTechnology
    {
        [Key]
        [Required]
        public Guid WBUTechnologyID { get; set; }

        public Guid WorkloadBacklogWBID { get; set; }

        [ForeignKey("WorkloadBacklogWBID")]
        public virtual WorkloadBacklog WorkloadBacklog { get; set; }

        public Guid TechnologyTechnologyId { get; set; }

        [ForeignKey("TechnologyTechnologyId")]
        public virtual Technology Technology { get; set; }
    }
}
