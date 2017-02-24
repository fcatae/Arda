using System.Collections.Generic;

namespace Arda.Common.ViewModels.Main
{
    public class ResourcesViewModel
    {
        public string Category { get; set; }

        public IEnumerable<string> Resources { get; set; }

    }
}
