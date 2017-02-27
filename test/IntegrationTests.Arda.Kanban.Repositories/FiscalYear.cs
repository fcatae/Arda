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
            FiscalYearRepository fiscalYear = new FiscalYearRepository(ArdaTestMgr.GetDbContext());

            var list = fiscalYear.GetAllFiscalYears();

            ArdaTestMgr.CheckResult(list);
        }
    }
}
