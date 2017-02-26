using System;
using Xunit;
using Arda.Kanban.Models;
using Arda.Kanban.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests
{
    public class FiscalYear
    {
        KanbanContext GetDbContext()
        {
            var opts = (new DbContextOptionsBuilder<KanbanContext>())
                            .UseSqlServer("connestring");

            return new KanbanContext(opts.Options);
        }

        [Fact]
        public void GetAllFiscalYears_Should_ReturnAllValues() 
        {
            var dbContext = GetDbContext();

            FiscalYearRepository fiscalYear = new FiscalYearRepository(dbContext);

            var list = fiscalYear.GetAllFiscalYears();

            Assert.True(true);
        }
    }
}
