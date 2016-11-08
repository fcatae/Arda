using Newtonsoft.Json;
using System.Collections.Generic;

namespace Arda.Common.ViewModels.Permissions
{

    public class CacheViewModel
    {
        public string Code { get; set; }

        public ICollection<PermissionsToBeCachedViewModel> Permissions { get; set; }

        public CacheViewModel() { }

        public CacheViewModel(string propertiesCachedSerialized)
        {
            var prop = JsonConvert.DeserializeObject<CacheViewModel>(propertiesCachedSerialized);

            Code = prop.Code;
            Permissions = prop.Permissions;
        }

        public CacheViewModel(string code, ICollection<PermissionsToBeCachedViewModel> permission)
        {
            Code = code;
            Permissions = permission;
        }

        public CacheViewModel(ICollection<PermissionsToBeCachedViewModel> permission)
        {
            Code = string.Empty;
            Permissions = permission;
        }


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}