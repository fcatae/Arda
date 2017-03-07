using System;
using System.Linq;
using Xunit;
using System.Collections.Generic;
using Arda.Kanban.Models;
using Arda.Kanban.Repositories;
using Arda.Kanban.ViewModels;

namespace IntegrationTests
{
    public class Actitivy : ISupportSnapshot<ActivityViewModel>
    {
        public IEnumerable<ActivityViewModel> GetSnapshot(TransactionalKanbanContext context)
        {
            ActivityRepository activiy = new ActivityRepository(context);

            return activiy.GetAllActivities().ToArray();
        }

        [Fact]
        public void Actitivy_GetAllActivities() 
        {
            ArdaTestMgr.Validate(this, $"Actitivy.GetAllActivities()",
                (list, ctx) => {
                    var rows = from r in list
                               select r.ActivityName;

                    return rows;
                });
        }        
    }
}
