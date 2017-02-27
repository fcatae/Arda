using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arda.Kanban.Models
{
    [Table("Technologies")]
    public class Technology
    {
        [Key]
        [Required]
        public Guid TechnologyID { get; set; }

        [Required]
        public string TechnologyName { get; set; }

        public virtual ICollection<WorkloadBacklogTechnology> WBTechnologies { get; set; }
    }
}
