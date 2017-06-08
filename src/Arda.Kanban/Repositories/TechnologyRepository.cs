using System.Collections.Generic;
using System.Linq;
using Arda.Kanban.Models;
using Arda.Kanban.Models.Repositories;
using Arda.Kanban.ViewModels;

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
