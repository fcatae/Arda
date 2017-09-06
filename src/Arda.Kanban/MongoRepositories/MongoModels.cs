using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arda.Kanban.MongoRepositories
{
    public class Activity
    {
        public string _id { get; set; }
        public string activityName { get; set; }
    }

    public class FiscalYear
    {
        public string _id { get; set; }

        public int fullNumericFiscalYear { get; set; }

        public string textualFiscalYear { get; set; }
    }

    public class Technology
    {
        public string _id { get; set; }

        public string technologyName { get; set; }
    }
}
