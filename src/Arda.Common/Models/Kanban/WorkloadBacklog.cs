using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Arda.Common.Models.Kanban
{
    [Table("WorkloadBacklogs")]
    public class WorkloadBacklog
    {
        //Entity Properties:
        [Key]
        [Required]
        public Guid WBID { get; set; }

        [Required]
        public bool WBIsWorkload { get; set; }

        [Required]
        public Status WBStatus { get; set; }

        [Required]
        public DateTime WBStartDate { get; set; }

        [Required]
        public DateTime WBEndDate { get; set; }

        [Required]
        public string WBTitle { get; set; }
        
        public string WBDescription { get; set; }

        [Required]
        public virtual Expertise WBExpertise { get; set; }

        [Required]
        public Complexity WBComplexity { get; set; }

        [Required]
        public string WBCreatedBy { get; set; }

        [Required]
        public DateTime WBCreatedDate { get; set; }


        //Foreign Keys:
        public Guid WBActivityActivityID { get; set; }

        [ForeignKey("WBActivityActivityID")]
        public Activity WBActivity { get; set; }

        public ICollection<File> WBFiles { get; set; }

        public ICollection<WorkloadBacklogMetric> WBMetrics { get; set; }
        
        public ICollection<WorkloadBacklogUser> WBUsers { get; set; }

        public ICollection<WorkloadBacklogTechnology> WBTechnologies { get; set; }

        public ICollection<Appointment> WBAppointments { get; set; }

    }

    public enum Status
    {
        To_Do,
        Doing,
        Done,
        Approved
    }

    public enum Expertise
    {
        Management,
        Project,
        [Display(Name = "Program Management")]
        ProgramManagement,
        Architecture,
        Coding,
        [Display(Name = "Building Infra")]
        BuildingInfra,
        [Display(Name = "Internal Processes")]
        InternalProcesses
    }

    public enum Complexity
    {
        VeryLow,
        Low,
        Medium,
        Hard,
        VeryHard
    }
}