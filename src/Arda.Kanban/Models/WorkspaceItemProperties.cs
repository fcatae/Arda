using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arda.Kanban.Models
{
    public class WorkspaceItemProperties
    {
        public string Description { get; set; }
        public bool? IsWorkload { get; set; }
        public int? Expertise { get; set; }
        public int? WBComplexity { get; set; }
        public Guid? ActivityID { get; set; }
        public Guid? LastAppointmentId { get; set; }

        public IEnumerable<string> Files { get; set; }
        public IEnumerable<string> WorkloadUsers { get; set; }
        public IEnumerable<Guid> Metrics { get; set; }
        public IEnumerable<Guid> Technologies { get; set; }
    }
}
