using System;
using System.Linq;
using Xunit;
using System.Collections.Generic;
using Arda.Kanban.Models;
using Arda.Kanban.Repositories;
using Arda.Common.ViewModels.Main;

namespace IntegrationTests
{
    public class Workload : ISupportSnapshot<WorkloadViewModel>
    {
        public IEnumerable<WorkloadViewModel> GetSnapshot(TransactionalKanbanContext context)
        {
            WorkloadRepository workload = new WorkloadRepository(context);

            return workload.GetAllWorkloads().ToArray();
        }

        [Fact]
        public void Workload_GetAllWorkloads_Should_ReturnAllValues() 
        {
            ArdaTestMgr.Validate(this, $"Workload.GetAllWorkloads()",
                (list, ctx) => {
                    var rows = from r in list
                               select r.WBTitle;

                    return rows;
                });
        }

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
        //    string GUID = "{d38759ab-e310-46f0-a6c3-b0594c2531ab}";
        //    int YEAR = 2021;
        //    string YEARTXT = "TEST-2021";

        //    ArdaTestMgr.Validate(this, $"FiscalYear.EditFiscalYearByID({GUID},{YEAR},{YEARTXT})",
        //        (list, ctx) => {
        //            FiscalYearRepository fiscalYear = new FiscalYearRepository(ctx);

        //            var row = list[0];

        //            row.FiscalYearID = Guid.Parse(GUID);
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
        //    }
    }
}
