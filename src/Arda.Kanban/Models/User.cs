using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arda.Kanban.Models
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
