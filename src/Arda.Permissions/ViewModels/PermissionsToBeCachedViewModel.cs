namespace Arda.Permissions.ViewModels
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