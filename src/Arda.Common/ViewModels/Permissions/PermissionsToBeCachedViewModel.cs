using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arda.Common.ViewModels.Permissions
{
    public class PermissionsToBeCachedViewModel
    {
        public string Endpoint { get; set; }
        public string Module { get; set; }
        public string Resource { get; set; }
        public string Category { get; set; }
        public string DisplayName { get; set; }
    }
}