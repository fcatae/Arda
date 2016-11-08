using Arda.Common.ViewModels.Main;
using System.Collections.Generic;

namespace Arda.Common.Interfaces.Kanban
{
    public interface ITechnologyRepository
    {       
        // Return a list of all technologies.
        IEnumerable<TechnologyViewModel> GetAllTechnologies();
    }
}
