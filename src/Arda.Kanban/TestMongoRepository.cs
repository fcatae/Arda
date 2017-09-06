using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Arda.Kanban.MongoRepositories;

namespace Arda.Kanban
{
    public static class TestMongoRepository
    {
        public static void InitialSeed(MongoContext context)
        {
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
                var fy17 = new ViewModels.FiscalYearViewModel
                {
                    FiscalYearID = Guid.NewGuid(),
                    FullNumericFiscalYearMain = 2017,
                    TextualFiscalYearMain = "FY2017"
                };

                var fy18 = new ViewModels.FiscalYearViewModel
                {
                    FiscalYearID = Guid.NewGuid(),
                    FullNumericFiscalYearMain = 2018,
                    TextualFiscalYearMain = "FY2018"
                };

                fiscalyearRepo.AddNewFiscalYear(fy17);
                fiscalyearRepo.AddNewFiscalYear(fy18);

                fiscalYears = fiscalyearRepo.GetAllFiscalYears();
            }

            var technologiesRepo = new TechnologyMongoRepository(context);
            var technologies = technologiesRepo.GetAllTechnologies();

            if( !technologies.Any() )
            {
                technologiesRepo.Create(Guid.NewGuid(), "Cloud Technologies");
            }

            var metricsRepo = new MetricMongoRepository(context);
            var metrics = metricsRepo.GetAllMetrics();

            if( !metrics.Any() )
            {
                var fy17 = fiscalYears[0];
                var fy18 = fiscalYears[1];

                var m1 = new ViewModels.MetricViewModel()
                {
                    MetricID = Guid.NewGuid(),
                    MetricName = "Product sales",
                    Description = "$$$ Dollars $$$",
                    MetricCategory = "Sales",
                    FiscalYearID = fy17.FiscalYearID
                };
                var m2 = new ViewModels.MetricViewModel()
                {
                    MetricID = Guid.NewGuid(),
                    MetricName = "Customer satisfaction",
                    Description = "Happy customers",
                    MetricCategory = "Customer",
                    FiscalYearID = fy18.FiscalYearID
                };

                metricsRepo.AddNewMetric(m1);
                metricsRepo.AddNewMetric(m2);
            }
        }

        public static void Test()
        {
            var context = new MongoContext();

            context.DeleteAll();

            InitialSeed(context);

            TestActivities(context);
            TestFiscalYears(context);
            TestTechnologies(context);
            TestMetrics(context);
        }

        public static void TestActivities(MongoContext context)
        {
            var activityRepo = new ActivityMongoRepository(context);

            var activities = activityRepo.GetAllActivities();

            // assert
            Assert(activities.Count(), 1);
        }

        public static void TestFiscalYears(MongoContext context)
        {
            var fiscalyearRepo = new FiscalYearMongoRepository(context);
            var fiscalYears = fiscalyearRepo.GetAllFiscalYears();

            var id17 = fiscalYears[0].FiscalYearID;
            var id18 = fiscalYears[1].FiscalYearID;
            var fy18 = fiscalyearRepo.GetFiscalYearByID(id18);

            var editId = id18;
            var fy16 = new ViewModels.FiscalYearViewModel()
            {
                FiscalYearID = editId,
                TextualFiscalYearMain = "FY2016",
                FullNumericFiscalYearMain = 2016
            };                
            fiscalyearRepo.EditFiscalYearByID(fy16);

            fiscalyearRepo.DeleteFiscalYearByID(id17);

            var finalFiscalYears = fiscalyearRepo.GetAllFiscalYears();

            // assert
            Assert(finalFiscalYears.Count, 1);
            Assert(finalFiscalYears[0].FullNumericFiscalYearMain, 2016);
        }

        public static void TestTechnologies(MongoContext context)
        {
            var technologiesRepo = new TechnologyMongoRepository(context);

            var technologies = technologiesRepo.GetAllTechnologies();

            // assert
            Assert(technologies.Count(), 1);
        }

        public static void TestMetrics(MongoContext context)
        {
            var metricsRepo = new MetricMongoRepository(context);
            var metrics = metricsRepo.GetAllMetrics();

            var id1 = metrics[0].MetricID;
            var id2 = metrics[1].MetricID;
            
            // assert
            Assert(metrics.Count, 2);
            
        }
        static void Assert(object a, object b)
        {
            if( !a.Equals(b) )
                throw new InvalidOperationException();
        }
    }
}
