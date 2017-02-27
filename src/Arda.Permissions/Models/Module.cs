using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Arda.Permissions.Models
{
    [DebuggerDisplay("ID={ModuleID}, Endpoint={Endpoint}/{ModuleName}")]
    [Table("Modules")]
    public class Module
    {
        [Key]
        public int ModuleID { get; set; }

        [Required]
        public string Endpoint { get; set; }

        [Required]
        public string ModuleName { get; set; }
        

        public virtual ICollection<Resource> Resources { get; set; }
    }
}