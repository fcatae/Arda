using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arda.Common.ViewModels.Main
{
    public class WorkloadViewModel
    {
        public Guid WBID { get; set; }

        public bool WBIsWorkload { get; set; }
        
        public int WBStatus { get; set; }

        public DateTime WBStartDate { get; set; }

        public DateTime WBEndDate { get; set; }

        public string WBTitle { get; set; }

        public string WBDescription { get; set; }

        public int WBExpertise { get; set; }

        public int WBComplexity { get; set; }
        
        public string WBCreatedBy { get; set; }
        
        public DateTime WBCreatedDate { get; set; }

        
        public Guid WBActivity { get; set; }

        //GUID, URL and Name:
        public IEnumerable<Tuple<Guid, string, string>> WBFilesList { get; set; }

        public IEnumerable<Guid> WBMetrics { get; set; }

        public IEnumerable<string> WBUsers { get; set; }

        public IEnumerable<Guid> WBTechnologies { get; set; }
        
    }
}
