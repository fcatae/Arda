using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arda.Common.ViewModels.Reports
{
    public class ActivityConsumingViewModel
    {
        public string Activity { get; set; }

        public int Hours { get; set; }

        public decimal Percent { get; set; }
    }
}
