using Arda.Common.Interfaces.Kanban;
using System;
using System.Collections.Generic;
using System.Linq;
using Arda.Common.ViewModels.Main;
using Arda.Kanban.Models;

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
            try
            {
                var response = (from t in _context.Technologies
                                orderby t.TechnologyName
                                select new TechnologyViewModel
                                {
                                    TechnologyID = t.TechnologyID,
                                    TechnologyName = t.TechnologyName
                                }).ToList();

                if (response != null)
                {
                    return response;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                // Shouldn't this exception be logged in someform?
                return null;
            }
        }
    }
}
