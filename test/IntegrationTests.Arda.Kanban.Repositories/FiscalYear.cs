using System;
using System.Linq;
using Xunit;
using Arda.Kanban.Models;
using Arda.Kanban.Repositories;
using System.Collections.Generic;

namespace IntegrationTests
{
    public class FiscalYear
    {
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
            using (var context = ArdaTestMgr.GetTransactionContext())
            {
                FiscalYearRepository fiscalYear = new FiscalYearRepository(context);

                var list = fiscalYear.GetAllFiscalYears().ToArray();
                var fiscalYearId = list.Min(r => r.FiscalYearID);

                var row = fiscalYear.GetFiscalYearByID(fiscalYearId);

                ArdaTestMgr.CheckResult(row);
            }
        }

        [Fact]
        public void FiscalYear_AddNewFiscalYear_Should_AddRow()
        {
            using (var context = ArdaTestMgr.GetTransactionContext())
            {
                FiscalYearRepository fiscalYear = new FiscalYearRepository(context);

                var before = fiscalYear.GetAllFiscalYears().ToArray();

                // Add row
                var row = before[0];

                Guid fiscalYearGuid = Guid.Parse("{aaaa0000-622a-4656-85df-39edc26be080}");

                row.FiscalYearID = fiscalYearGuid;
                row.FullNumericFiscalYearMain = 2021;
                row.TextualFiscalYearMain = "TEST-2021";

                fiscalYear.AddNewFiscalYear(row);

                var after = fiscalYear.GetAllFiscalYears().ToArray();

                Assert.Equal(before.Length, after.Length - 1);

                var newrow = (from r in after
                              where r.FiscalYearID == fiscalYearGuid
                              select r).ToArray()[0];

                ArdaTestMgr.CheckResult(row);
            }

        }

    }
}
