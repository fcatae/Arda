using System.Collections.Generic;
using Arda.Kanban.ViewModels;

namespace Arda.Kanban.Models.Repositories
{
    public interface ITechnologyRepository
    {       
        // Return a list of all technologies.
        IEnumerable<TechnologyViewModel> GetAllTechnologies();
    }
}
