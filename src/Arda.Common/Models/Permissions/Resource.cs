using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Arda.Common.Models.Permissions
{
    [DebuggerDisplay("ID={ResourceID}, Name={ResourceName}")]
    [Table("Resources")]
    public class Resource
    {
        [Key]
        public int ResourceID { get; set; }

        public int ModuleID { get; set; }

        [Required]
        public string ResourceName { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public int CategorySequence { get; set; }

        [Required]
        public string DisplayName { get; set; }

        [Required]
        public int ResourceSequence { get; set; }

        [ForeignKey("ModuleID")]
        public virtual Module Module { get; set; }

        public virtual ICollection<UsersPermission> UserPermissions { get; set; }
    }
}
