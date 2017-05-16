using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arda.Kanban.Models
{
    public class WorkspaceItemProperties
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
        public bool? IsWorkload { get; set; }
        public int? Expertise { get; set; }
        public int? Complexity { get; set; }
        public Guid? ActivityID { get; set; }
        public Guid? LastAppointmentId { get; set; }

        public IEnumerable<string> WorkloadUsers { get; set; }
        public IEnumerable<Guid> Metrics { get; set; }
        public IEnumerable<Guid> Technologies { get; set; }

        public IEnumerable<string> Files { get; set; }
    }

    public class WorkspaceItemPropertiesFilter
    {
        public bool HasActivityID { get; set; }
        public bool HasStartDate { get; set; }
        public bool HasEndDate { get; set; }
        public bool HasDescription { get; set; }
        public bool HasExpertise { get; set; }
        public bool HasIsWorkload { get; set; }
        public bool HasLastAppointmentId { get; set; }
        public bool HasMetrics { get; set; }
        public bool HasTechnologies { get; set; }
        public bool HasComplexity { get; set; }
        public bool HasWorkloadUsers { get; set; }
        public bool HasFiles { get; set; }
    }
}
