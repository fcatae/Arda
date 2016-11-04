using Arda.Common.ViewModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arda.Common.Interfaces.Kanban
{
    public interface ITechnologyRepository
    {       
        // Return a list of all technologies.
        IEnumerable<TechnologyViewModel> GetAllTechnologies();
    }
}
