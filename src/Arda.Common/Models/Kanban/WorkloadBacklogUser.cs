using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace Arda.Common.Models.Kanban
{
    [Table("WorkloadBacklogUsers")]
    public class WorkloadBacklogUser
    {
        [Key]
        [Required]
        public Guid WBUserID { get; set; }

        public Guid WorkloadBacklogWBID { get; set; }

        [ForeignKey("WorkloadBacklogWBID")]
        public virtual WorkloadBacklog WorkloadBacklog { get; set; }

        public string UserUniqueName { get; set; }

        [ForeignKey("UserUniqueName")]        
        public virtual User User { get; set; }
    }
}
