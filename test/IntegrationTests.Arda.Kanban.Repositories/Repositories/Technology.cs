using System;
using System.Linq;
using Xunit;
using System.Collections.Generic;
using Arda.Kanban.Models;
using Arda.Kanban.Repositories;
using Arda.Common.ViewModels.Main;

namespace IntegrationTests
{
    public class Technology : ISupportSnapshot<TechnologyViewModel>
    {
        public IEnumerable<TechnologyViewModel> GetSnapshot(TransactionalKanbanContext context)
        {
            TechnologyRepository technology = new TechnologyRepository(context);

            return technology.GetAllTechnologies().ToArray();
        }

        [Fact]
        public void Technology_GetAllTechnologies() 
        {
            ArdaTestMgr.Validate(this, $"Technology.GetAllTechnologies()",
                (list, ctx) => {
                    var rows = from r in list
                               select r.TechnologyName;

                    return rows;
                });
        }        
    }
}
