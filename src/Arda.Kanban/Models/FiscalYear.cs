using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arda.Kanban.Models
{
    [Table("FiscalYears")]
    public class FiscalYear
    {
        [Key]
        [Required]
        public Guid FiscalYearID { get; set; }

        [Required]
        public int FullNumericFiscalYear { get; set; }

        [Required]
        public string TextualFiscalYear { get; set; }


        public virtual ICollection<Metric> Metrics { get; set; }
    }
}
