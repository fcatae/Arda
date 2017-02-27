using System;
using System.Collections.Generic;
using System.Linq;
using Arda.Common.ViewModels.Main;
using Arda.Kanban.Models;
using Arda.Kanban.Models.Repositories;

namespace Arda.Kanban.Repositories
{
    public class TechnologyRepository : ITechnologyRepository
    {
        private KanbanContext _context;

        public TechnologyRepository(KanbanContext context)
        {
            _context = context;
        }


        public IEnumerable<TechnologyViewModel> GetAllTechnologies()
        {
            var response = (from t in _context.Technologies
                            orderby t.TechnologyName
                            select new TechnologyViewModel
                            {
                                TechnologyID = t.TechnologyID,
                                TechnologyName = t.TechnologyName
                            }).ToList();

            return response;
        }
    }
}
