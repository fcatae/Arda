using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arda.Common.Models.Permissions
{
    [Table("UsersPermissions")]
    public class UsersPermission
    {
        [Key]
        public int PermissionID { get; set; }

        public string UniqueName { get; set; }

        public int ResourceID { get; set; }

        [ForeignKey("ResourceID")]
        public virtual Resource Resource { get; set; }

        [ForeignKey("UniqueName")]
        public virtual User User { get; set; }
    }
}