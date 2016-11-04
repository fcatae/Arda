using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Arda.Common.Models.Kanban
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
