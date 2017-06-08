using System.Collections.Generic;
using Arda.Kanban.ViewModels;

namespace Arda.Kanban.Models.Repositories
{
    public interface IActivityRepository
    {
        // Return a list of all activities.
        IEnumerable<ActivityViewModel> GetAllActivities();
    }
}
