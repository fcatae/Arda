using System.Collections.Generic;
using Arda.Common.ViewModels.Main;

namespace Arda.Kanban.Models.Repositories
{
    public interface ITechnologyRepository
    {       
        // Return a list of all technologies.
        IEnumerable<TechnologyViewModel> GetAllTechnologies();
    }
}
