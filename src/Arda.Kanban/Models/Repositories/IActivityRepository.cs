using System.Collections.Generic;
using Arda.Common.ViewModels.Main;

namespace Arda.Kanban.Models.Repositories
{
    public interface IActivityRepository
    {
        // Return a list of all activities.
        IEnumerable<ActivityViewModel> GetAllActivities();
    }
}
