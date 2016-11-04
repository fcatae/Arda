using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Arda.Common.Models.Kanban
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
