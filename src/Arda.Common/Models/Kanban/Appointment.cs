using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Arda.Common.Models.Kanban
{
    [Table("Appointments")]
    public class Appointment
    {
        [Key]
        [Required]
        public Guid AppointmentID { get; set; }

        [Required]
        public string AppointmentUserUniqueName { get; set; }

        [ForeignKey("AppointmentUserUniqueName")]
        public virtual User AppointmentUser { get; set; }

        [Required]
        public Guid AppointmentWorkloadWBID { get; set; }

        [ForeignKey("AppointmentWorkloadWBID")]
        public virtual WorkloadBacklog AppointmentWorkload { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public int AppointmentHoursDispensed { get; set; }

        public decimal AppointmentTE { get; set; }

        public string AppointmentComment { get; set; }
    }
}
