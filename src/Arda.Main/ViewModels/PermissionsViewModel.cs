using System.Collections.Generic;

namespace Arda.Main.ViewModels
{
    public class PermissionsViewModel
    {
        public List<Permission> permissions { get; set; }
    }

    public class Permission
    {
        public string category { get; set; }
        public string resource { get; set; }
    }

}
