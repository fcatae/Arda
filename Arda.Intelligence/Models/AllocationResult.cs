using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arda.Intelligence.Models
{
    public class AllocationResult
    {
        public string Professional { get; set; }
        public int Hours_Last_7_Days { get; set; }
        public decimal Perc_Allocation { get; set; }
    }
}
