using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Arda.Kanban.MongoRepositories
{
    public class MongoContext
    {
        public IMongoCollection<Activity> Activities { get; private set; }
        public IMongoCollection<FiscalYear> FiscalYear { get; private set; }

        public MongoContext()
        {
            var client = new MongoClient();
            var db = client.GetDatabase("arda");
            this.Activities = db.GetCollection<Activity>("activities");
            this.FiscalYear = db.GetCollection<FiscalYear>("fiscalyear");
        }
    }

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
}
