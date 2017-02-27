using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arda.Kanban.Models
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
