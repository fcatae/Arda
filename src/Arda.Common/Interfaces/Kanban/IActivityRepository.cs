using Arda.Common.ViewModels.Main;
using System.Collections.Generic;

namespace Arda.Common.Interfaces.Kanban
{
    public interface IActivityRepository
    {
        // Return a list of all activities.
        IEnumerable<ActivityViewModel> GetAllActivities();
    }
}
