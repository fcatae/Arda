using System;
using System.Collections.Generic;

namespace Arda.Main.ViewModels
{
    public class WorkloadsByUserViewModel
    {
        public Guid _WorkloadID { get; set; }

        public string _WorkloadTitle { get; set; }

        public DateTime _WorkloadStartDate { get; set; }

        public DateTime _WorkloadEndDate { get; set; }

        public int _WorkloadStatus { get; set; }

        public IEnumerable<Tuple<string, string>> _WorkloadUsers { get; set; }

        public int _WorkloadHours { get; set; }

        public int _WorkloadAttachments { get; set; }

        public string _WorkloadExpertise { get; set; }

        public bool _WorkloadIsWorkload { get; set; }
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
