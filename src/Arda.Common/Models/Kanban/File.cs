using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Arda.Common.Models.Kanban
{
    [Table("Files")]
    public class File
    {
        [Key]
        [Required]
        public Guid FileID { get; set; }

        [Required]
        public string FileLink { get; set; }

        [Required]
        public string FileName { get; set; }

        public string FileDescription { get; set; }

        public Guid WorkloadBacklogWBID { get; set; }

        [ForeignKey("WorkloadBacklogWBID")]
        public virtual WorkloadBacklog WorkloadBacklog { get; set; }
    }
}
