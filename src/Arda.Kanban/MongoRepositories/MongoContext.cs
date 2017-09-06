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
        string DBNAME = "arda";

        public IMongoCollection<Activity> Activities { get; private set; }
        public IMongoCollection<FiscalYear> FiscalYears { get; private set; }
        public IMongoCollection<Technology> Technologies { get; private set; }
        public IMongoCollection<Metric> Metrics { get; private set; }
        public IMongoCollection<User> Users { get; private set; }

        public MongoContext()
        {
            var client = new MongoClient();
            var db = client.GetDatabase(DBNAME);
            this.Activities = db.GetCollection<Activity>("activities");
            this.FiscalYears = db.GetCollection<FiscalYear>("fiscalyears");
            this.Technologies = db.GetCollection<Technology>("technologies");
            this.Metrics = db.GetCollection<Metric>("metrics");
            this.Users = db.GetCollection<User>("users");
        }

        public void DeleteAll()
        {
            var client = new MongoClient();
            client.DropDatabase(DBNAME);
        }
    }
}
