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

        public void TestItemLogRepository()
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

            var initial = workspace.GetLogs(item.Id);

            workspace.AppendLog(item.Id, new WorkspaceItemLog() {
                Id = Guid.NewGuid(),
                CreatedBy = item.CreatedBy,
                CreatedDate = DateTime.Now,
                Text = "teste 1",
                Properties = null            
            });
            workspace.AppendLog(item.Id, new WorkspaceItemLog()
            {
                Id = Guid.NewGuid(),
                CreatedBy = item.CreatedBy,
                CreatedDate = DateTime.Now,
                Text = "teste 2",
                Properties = new WorkspaceItemLogProperties()
                {
                    Expenses = 1,
                    LaborHours = 8
                }
            });

            var final = workspace.GetLogs(item.Id);

            workspace.Delete(item.Id);
        }

        public void TestGetworkload()
        {
            WorkspaceRepository workspace = new WorkspaceRepository(_context);

            var a = workspace.GetWorkloadItemProperties(Guid.Parse("fe77fbf1-6176-4c06-93a4-6e32cb28d165"),
                new WorkspaceItemPropertiesFilter()
                {
                    HasDescription = true,
                    HasIsWorkload = true,
                    HasWorkloadUsers = true
                }
                );

            var r = workspace.TryGet(
                Guid.Parse("fe77fbf1-6176-4c06-93a4-6e32cb28d165"), 
                new WorkspaceItemPropertiesFilter()
                {
                    HasDescription = true,
                    HasIsWorkload = true,
                    HasWorkloadUsers = true
                });

            var r2 = workspace.List(new WorkspaceItemPropertiesFilter()
            {
                HasDescription = true,
                HasIsWorkload = true,
                HasWorkloadUsers = true
            });

            foreach(var i in r2)
            {
                var u = i.Properties.WorkloadUsers;
                var user = i.Properties.WorkloadUsers.ToArray();
            }
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
