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
            var activityRepo = new ActivityMongoRepository(context);
            var activities = activityRepo.GetAllActivities();

            // empty activities
            if( !activities.Any() )
            {
                activityRepo.Create(Guid.NewGuid(), "coding");
            }

            var fiscalyearRepo = new FiscalYearMongoRepository(context);
            var fiscalYears = fiscalyearRepo.GetAllFiscalYears();

            // empty activities
            if (!fiscalYears.Any())
            {
                var fy = new ViewModels.FiscalYearViewModel
                {
                    FiscalYearID = Guid.NewGuid(),
                    FullNumericFiscalYearMain = 2017,
                    TextualFiscalYearMain = "FY2017"
                };

                fiscalyearRepo.AddNewFiscalYear(fy);
            }
        }

        public static void Test()
        {
            InitialSeed();

            var context = new MongoContext();
            var activityRepo = new ActivityMongoRepository(context);

            var activities = activityRepo.GetAllActivities();

            var fiscalyearRepo = new FiscalYearMongoRepository(context);
            var fiscalYears = fiscalyearRepo.GetAllFiscalYears();

        }
    }
}
