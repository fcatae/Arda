using System;
using System.Collections.Generic;
using System.Linq;
using Arda.Kanban.Models;
using Arda.Kanban.Models.Repositories;
using Arda.Kanban.ViewModels;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Arda.Kanban.MongoRepositories
{
    public class ActivityMongoRepository : IActivityRepository
    {
        MongoContext _context;
        BsonDocument all = new BsonDocument() { };
        BsonDocument byActivityName = new BsonDocument() { { nameof(Activity.activityName), 1 } };

        public ActivityMongoRepository(MongoContext context)
        {
            this._context = context;
        }

        public void Create(Guid activityId, string activityName)
        {
            var activity = new Activity
            {
                _id = activityId.ToString(),
                activityName = activityName
            };

            _context.Activities.InsertOne(activity);
        }

        public IEnumerable<ActivityViewModel> GetAllActivities()
        {
            var response = (from a in _context.Activities.Find(all).Sort(byActivityName).ToEnumerable()
                           select new ActivityViewModel
                           {
                               ActivityID = Guid.Parse( a._id ),
                               ActivityName = a.activityName
                           }).ToList();
            
            return response;
        }
    }
}
