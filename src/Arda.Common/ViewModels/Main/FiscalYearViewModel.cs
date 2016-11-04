using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arda.Common.ViewModels.Main
{
    public class FiscalYearViewModel
    {
        public Guid FiscalYearID { get; set; }

        public int FullNumericFiscalYearMain { get; set; }

        public string TextualFiscalYearMain { get; set; }
    }
}
