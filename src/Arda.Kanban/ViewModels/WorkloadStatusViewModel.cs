using System;
using System.Collections.Generic;

namespace Arda.Kanban.ViewModels
{
    public class WorkloadStatusViewModel
    {
        public Guid WorkloadID { get; set; }

        public string Title { get; set; }

        public string StatusText { get; set; }

        public int State { get; set; }

        public IEnumerable<string> Users { get; set; }        
    }

    public class WorkloadStatusCompatViewModel
    {
        // renamed fields
        public Guid _WorkloadID { get; set; }
        public string _WorkloadTitle { get; set; }
        public int _WorkloadStatus { get; set; }
        public IEnumerable<Tuple<string, string>> _WorkloadUsers { get; set; }

        // status field
        public string StatusText { get; set; }

        // unused        
        public DateTime _WorkloadStartDate { get; set; }
        public DateTime _WorkloadEndDate { get; set; }
        public int _WorkloadHours { get; set; }
        public int _WorkloadAttachments { get; set; }
        public string _WorkloadExpertise { get; set; }
        public bool _WorkloadIsWorkload { get; set; }
    }
}
