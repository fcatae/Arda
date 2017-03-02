using System.Collections.Generic;

namespace Arda.Permissions.ViewModels
{
    public class ResourcesViewModel
    {
        public string Category { get; set; }

        public IEnumerable<string> Resources { get; set; }

    }
}
