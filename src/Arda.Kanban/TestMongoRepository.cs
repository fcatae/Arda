using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Arda.Kanban.MongoRepositories;

namespace Arda.Kanban
{
    public static class TestMongoRepository
    {
        public static void InitialSeed()
        {
            var context = new MongoContext();
            var repo = new ActivityMongoRepository(context);
            var activities = repo.GetAllActivities();

            // empty activities
            if( !activities.Any() )
            {
                repo.Create(Guid.NewGuid(), "coding");
            }
        }

        public static void Test()
        {
            InitialSeed();

            var context = new MongoContext();
            var activityRepo = new ActivityMongoRepository(context);

            var activities = activityRepo.GetAllActivities();
        }        
    }
}
