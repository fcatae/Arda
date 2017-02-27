using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arda.Kanban.Models
{
    [Table("Activities")]
    public class Activity
    {
        [Key]
        [Required]
        public Guid ActivityID { get; set; }

        [Required]
        public string ActivityName { get; set; }


        public virtual ICollection<WorkloadBacklog> WBs { get; set; }

    }
}
