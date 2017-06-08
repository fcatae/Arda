using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arda.Kanban.Models
{
    public class WorkspaceItemLog
    {
        public Guid Id { get; set; }
        public string Text { get; set; }

        // metadata
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        
        public WorkspaceItemLogProperties Properties { get; set; }
    }    
}
