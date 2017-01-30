using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arda.Intelligence.Models
{
    public class MLIPRInput
    {
        public List<string> ColumnNames { get; set; }
        public List<List<string>> Values { get; set; }
    }
}
