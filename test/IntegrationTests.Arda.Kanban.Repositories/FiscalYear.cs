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
            using (var context = ArdaTestMgr.GetTransactionContext())
            {
                FiscalYearRepository fiscalYear = new FiscalYearRepository(context);

                var list = fiscalYear.GetAllFiscalYears();

                ArdaTestMgr.CheckResult(list);
            }
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
            using (var context = ArdaTestMgr.GetTransactionContext())
            {
                FiscalYearRepository fiscalYear = new FiscalYearRepository(context);

                var before = fiscalYear.GetAllFiscalYears().ToArray();

                // Edit row
                var row = before[0];

                row.FullNumericFiscalYearMain = 2021;
                row.TextualFiscalYearMain = "TEST-2021";

                fiscalYear.EditFiscalYearByID(row);

                var after = fiscalYear.GetAllFiscalYears().ToArray();
                
                ArdaTestMgr.CheckResult(row);
            }

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
