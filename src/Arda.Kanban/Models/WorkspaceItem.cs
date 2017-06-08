using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arda.Kanban.Models
{
    public class WorkspaceItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public int ItemState { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // metadata
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        // move to properties?
        public string Description { get; set; }

        public WorkspaceItemProperties Properties { get; set; }
    }
}
