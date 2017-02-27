using System;
using System.Collections.Generic;
using System.Linq;
using Arda.Common.ViewModels.Main;
using Arda.Kanban.Models;
using Arda.Kanban.Models.Repositories;

namespace Arda.Kanban.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private KanbanContext _context;

        public ActivityRepository(KanbanContext context)
        {
            _context = context;
        }

        public IEnumerable<ActivityViewModel> GetAllActivities()
        {
            var response = (from a in _context.Activities
                            orderby a.ActivityName
                            select new ActivityViewModel
                            {
                                ActivityID = a.ActivityID,
                                ActivityName = a.ActivityName
                            }).ToList();

            return response;
        }
    }
}
