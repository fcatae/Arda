using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arda.Kanban.Models
{
    [Table("WorkloadBacklogTags")]
    public class WorkloadBacklogTags
    {
        [Key]
        [Required]
        public string TagId { get; set; }

        public Guid WorkloadBacklogWBID { get; set; }

        [ForeignKey("WorkloadBacklogWBID")]
        public virtual WorkloadBacklog WorkloadBacklog { get; set; }
    }
}
