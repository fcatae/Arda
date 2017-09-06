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
        public IMongoCollection<FiscalYear> FiscalYears { get; private set; }
        public IMongoCollection<Technology> Technologies { get; private set; }

        public MongoContext()
        {
            var client = new MongoClient();
            var db = client.GetDatabase("arda");
            this.Activities = db.GetCollection<Activity>("activities");
            this.FiscalYears = db.GetCollection<FiscalYear>("fiscalyears");
            this.Technologies = db.GetCollection<Technology>("technologies");
        }

        public void DeleteAll()
        {
            var client = new MongoClient();
            client.DropDatabase("arda");
        }
    }
}
