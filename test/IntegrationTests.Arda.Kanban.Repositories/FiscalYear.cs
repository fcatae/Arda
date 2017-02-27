using System;
using System.Linq;
using Xunit;
using System.Collections.Generic;
using Arda.Kanban.Models;
using Arda.Kanban.Repositories;
using Arda.Common.ViewModels.Main;

namespace IntegrationTests
{
    public class FiscalYear : ISupportSnapshot<FiscalYearViewModel>
    {
        public IEnumerable<FiscalYearViewModel> GetSnapshot(TransactionalKanbanContext context)
        {
            FiscalYearRepository fiscalYear = new FiscalYearRepository(context);

            return fiscalYear.GetAllFiscalYears().ToArray();
        }

        [Fact]
        public void FiscalYear_GetAllFiscalYears_Should_ReturnAllValues() 
        {
            ArdaTestMgr.Validate(this, $"FiscalYear.GetAllFiscalYears()",
                (list, ctx) => {
                    var rows = from r in list
                               select r.FullNumericFiscalYearMain;

                    return rows;
                });
        }

        [Fact]
        public void FiscalYear_GetFiscalYearByID_Should_ReturnExactlyOne()
        {
            int YEAR = 2018;

            ArdaTestMgr.Validate(this, $"FiscalYear.GetFiscalYearByID({YEAR})",
                (list, ctx) => {
                    FiscalYearRepository fiscalYear = new FiscalYearRepository(ctx);

                    var fiscalYearId = (from r in list
                                        where r.FullNumericFiscalYearMain == YEAR
                                        select r.FiscalYearID).First();

                    var row = fiscalYear.GetFiscalYearByID(fiscalYearId);

                    return row;
                });
        }

        [Fact]
        public void FiscalYear_AddNewFiscalYear_Should_AddRow()
        {
            string GUID = "{aaaa0000-622a-4656-85df-39edc26be080}";
            int YEAR = 2021;
            string YEARTXT = "TEST-2021";

            ArdaTestMgr.Validate(this, $"FiscalYear.AddNewFiscalYear({GUID},{YEAR},{YEARTXT})",
                (list, ctx) => {
                    FiscalYearRepository fiscalYear = new FiscalYearRepository(ctx);

                    var row = list[0];

                    Guid fiscalYearGuid = Guid.Parse(GUID);

                    row.FiscalYearID = fiscalYearGuid;
                    row.FullNumericFiscalYearMain = YEAR;
                    row.TextualFiscalYearMain = YEARTXT;

                    fiscalYear.AddNewFiscalYear(row);

                    return fiscalYear.GetAllFiscalYears();
                });
        }

        [Fact]
        public void FiscalYear_EditFiscalYearByID_Should_ChangeRow()
        {
            string GUID = "{e0fd2d01-020e-475f-9c28-a90f5d857877}";
            int YEAR = 2021;
            string YEARTXT = "TEST-2021";

            ArdaTestMgr.Validate(this, $"FiscalYear.EditFiscalYearByID({GUID},{YEAR},{YEARTXT})",
                (list, ctx) => {
                    FiscalYearRepository fiscalYear = new FiscalYearRepository(ctx);

                    var row = list[0];
                    
                    row.FullNumericFiscalYearMain = YEAR;
                    row.TextualFiscalYearMain = YEARTXT;

                    fiscalYear.EditFiscalYearByID(row);

                    return fiscalYear.GetAllFiscalYears();
                });            

        }



        [Fact]
        public void FiscalYear_DeleteFiscalYearByID_Should_ReturnRemoveExactlyOne()
        {
            using (var context = ArdaTestMgr.GetTransactionContext())
            {
                FiscalYearRepository fiscalYear = new FiscalYearRepository(context);

                var before = fiscalYear.GetAllFiscalYears().ToArray();

                // Remove row
                var fiscalYearId = (from row in before
                                    where row.FullNumericFiscalYearMain == 2018
                                    select row.FiscalYearID).First();
                                
                bool ret = fiscalYear.DeleteFiscalYearByID(fiscalYearId);

                Assert.True(ret);

                var after = fiscalYear.GetAllFiscalYears().ToArray();

                Assert.Equal(before.Length, after.Length + 1);

                ArdaTestMgr.CheckResult(after);
            }
        }

    }
}
