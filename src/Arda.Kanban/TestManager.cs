using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Arda.Kanban.Models;
using Arda.Kanban.Repositories;

namespace Arda.Kanban
{
    public class TestManager
    {
        private KanbanContext _context;

        public TestManager(KanbanContext context)
        {
            _context = context;
        }

        public void Run()
        {

        }

        public void CreateWorkspaceItem()
        {
            WorkspaceRepository workspace = new WorkspaceRepository(_context);

            WorkspaceItem item = new WorkspaceItem()
            {
                CreatedBy = "fcatae@microsoft.com", // must be a valid user (foreign key validation)
                CreatedDate = DateTime.Now,
                Description = "Description...",
                EndDate = null,
                Id = Guid.NewGuid(),
                ItemState = 2,
                StartDate = null,
                Summary = "Summary.",
                Title = "#title#"
            };

            workspace.Create(item);
        }

    }
}
