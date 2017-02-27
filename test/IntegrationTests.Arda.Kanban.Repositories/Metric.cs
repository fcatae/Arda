using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using System.Threading.Tasks;
using Arda.Kanban.Models;
using Arda.Kanban.Repositories;
using Arda.Common.ViewModels.Main;

namespace IntegrationTests
{
    public class Metric : ISupportSnapshot<MetricViewModel>
    {
        public IEnumerable<MetricViewModel> GetSnapshot(TransactionalKanbanContext context)
        {
            MetricRepository metrics = new MetricRepository(context);

            return metrics.GetAllMetrics();
        }

        [Fact]
        public void Metric_GetAllMetrics_Should_ReturnAllValues()
        {
            ArdaTestMgr.Validate(this, $"Metric.GetAllMetrics()",
                (list, ctx) => {
                    var rows = from r in list
                               select new { r.TextualFiscalYear, r.MetricName};

                    return rows;
                });
        }
        
        [Fact]
        public void Metric_GetAllMetrics_Should_ReturnFilteredData()
        {
            int YEAR = 2017;

            ArdaTestMgr.Validate(this, $"Metric.GetAllMetrics({YEAR})",
                (list, ctx) =>
                {
                    MetricRepository metric = new MetricRepository(ctx);

                    var row = metric.GetAllMetrics(YEAR);

                    return row;
                });
        }

        public void Metric_GetMetricByID_Should_ReturnSingleRow()
        {
            string METRIC_GUID = "{819193e6-ea01-4c4e-a948-fc44453b2604}"; // Education GUID

            ArdaTestMgr.Validate(this, $"Metric.GetMetricByID({METRIC_GUID})",
                (list, ctx) =>
                {
                    MetricRepository metric = new MetricRepository(ctx);

                    var row = metric.GetMetricByID(Guid.Parse(METRIC_GUID));

                    return row;
                });
        }
        
        [Fact]
        public void Metric_AddNewMetric_Should_AddRow()
        {
            string GUID = "{aaaa0011-622a-4656-85df-39edc26be080}";
            string METRIC_CATEGORY = "My Metric Category";
            string METRIC_NAME = "My Metric Name";
            string METRIC_DESCRIPTION = "My very long description of the metric blah blah blah...";
            string METRIC_FISCALYEAR_GUID = "c6a45416-81a2-4034-adac-c7eab5225ece"; // 2018

            ArdaTestMgr.Validate(this, $"Metric.AddNewMetric({GUID},{METRIC_CATEGORY},{METRIC_NAME},LongDescription, (Year:2018) {METRIC_FISCALYEAR_GUID})",
                (list, ctx) =>
                {
                    MetricRepository metric = new MetricRepository(ctx);

                    var row = new MetricViewModel()
                    {
                        MetricID = Guid.Parse(GUID),
                        MetricCategory = METRIC_CATEGORY,
                        MetricName = METRIC_NAME,
                        Description = METRIC_DESCRIPTION,
                        FiscalYearID = Guid.Parse(METRIC_FISCALYEAR_GUID),

                        // Ignored data
                        FullNumericFiscalYear = 0,
                        TextualFiscalYear = "IGNORED"
                    };

                    metric.AddNewMetric(row);

                    return metric.GetAllMetrics();
                });
        }

        // TODO:
        //EditMetricByID
        //DeleteMetricByID


        //[Fact]
        //public void FiscalYear_EditFiscalYearByID_Should_ChangeRow()
        //{
        //    string GUID = "{e0fd2d01-020e-475f-9c28-a90f5d857877}";
        //    int YEAR = 2021;
        //    string YEARTXT = "TEST-2021";

        //    ArdaTestMgr.Validate(this, $"FiscalYear.EditFiscalYearByID({GUID},{YEAR},{YEARTXT})",
        //        (list, ctx) => {
        //            FiscalYearRepository fiscalYear = new FiscalYearRepository(ctx);

        //            var row = list[0];

        //            row.FullNumericFiscalYearMain = YEAR;
        //            row.TextualFiscalYearMain = YEARTXT;

        //            fiscalYear.EditFiscalYearByID(row);

        //            return fiscalYear.GetAllFiscalYears();
        //        });

        //}



        //[Fact]
        //public void FiscalYear_DeleteFiscalYearByID_Should_ReturnRemoveExactlyOne()
        //{
        //    int YEAR = 2018;

        //    ArdaTestMgr.Validate(this, $"FiscalYear.DeleteFiscalYearByID({YEAR})",
        //        (list, ctx) => {
        //            FiscalYearRepository fiscalYear = new FiscalYearRepository(ctx);

        //            var fiscalYearId = (from row in list
        //                                where row.FullNumericFiscalYearMain == YEAR
        //                                select row.FiscalYearID).First();

        //            fiscalYear.DeleteFiscalYearByID(fiscalYearId);

        //            return fiscalYear.GetAllFiscalYears();
        //        });
        //}
    }
}
