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
}
