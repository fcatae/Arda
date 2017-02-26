using System;
using Xunit;
using Arda.Kanban.Models;
using Arda.Kanban.Repositories;

namespace IntegrationTests
{
    public class FiscalYear
    {
        [Fact]
        public void GetAllFiscalYears_Should_ReturnAllValues() 
        {
            var dbContext = ArdaTestMgr.GetDbContext();

            FiscalYearRepository fiscalYear = new FiscalYearRepository(dbContext);

            var list = fiscalYear.GetAllFiscalYears();

            Assert.True(true);
        }
    }
}
