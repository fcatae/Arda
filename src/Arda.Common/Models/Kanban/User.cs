using Arda.Common.Models.Kanban;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Arda.Common.Models.Kanban
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Required]
        public string UniqueName { get; set; }

        [Required]
        public string Name { get; set; }



        public virtual ICollection<WorkloadBacklogUser> WBUsers { get; set; }

        public virtual ICollection<Appointment> AppointmentUsers { get; set; }
    }
}
