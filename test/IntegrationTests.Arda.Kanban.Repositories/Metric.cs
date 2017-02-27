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

        // TODO:
        //AddNewMetric
        //GetAllMetrics
        //GetAllMetrics(int year)
        //GetMetricByID
        //EditMetricByID
        //DeleteMetricByID

        //[Fact]
        //public void FiscalYear_GetFiscalYearByID_Should_ReturnExactlyOne()
        //{
        //    int YEAR = 2018;

        //    ArdaTestMgr.Validate(this, $"FiscalYear.GetFiscalYearByID({YEAR})",
        //        (list, ctx) => {
        //            FiscalYearRepository fiscalYear = new FiscalYearRepository(ctx);

        //            var fiscalYearId = (from r in list
        //                                where r.FullNumericFiscalYearMain == YEAR
        //                                select r.FiscalYearID).First();

        //            var row = fiscalYear.GetFiscalYearByID(fiscalYearId);

        //            return row;
        //        });
        //}

        //[Fact]
        //public void FiscalYear_AddNewFiscalYear_Should_AddRow()
        //{
        //    string GUID = "{aaaa0000-622a-4656-85df-39edc26be080}";
        //    int YEAR = 2021;
        //    string YEARTXT = "TEST-2021";

        //    ArdaTestMgr.Validate(this, $"FiscalYear.AddNewFiscalYear({GUID},{YEAR},{YEARTXT})",
        //        (list, ctx) => {
        //            FiscalYearRepository fiscalYear = new FiscalYearRepository(ctx);

        //            var row = list[0];

        //            Guid fiscalYearGuid = Guid.Parse(GUID);

        //            row.FiscalYearID = fiscalYearGuid;
        //            row.FullNumericFiscalYearMain = YEAR;
        //            row.TextualFiscalYearMain = YEARTXT;

        //            fiscalYear.AddNewFiscalYear(row);

        //            return fiscalYear.GetAllFiscalYears();
        //        });
        //}

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
