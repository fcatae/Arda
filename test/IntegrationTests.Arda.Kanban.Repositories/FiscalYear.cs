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
            var dbContext = ArdaTestMgr.GetDbContext();

            FiscalYearRepository fiscalYear = new FiscalYearRepository(dbContext);

            var list = fiscalYear.GetAllFiscalYears();

            ArdaTestMgr.CheckResult(list);

            //string result = ArdaTestMgr.SerializeObject(list);
            //ArdaTestMgr.WriteFile("GetAllFiscalYears_Should_ReturnAllValues.json", result);
            //string expected = ArdaTestMgr.ReadFile("GetAllFiscalYears_Should_ReturnAllValues.json");           
            //Assert.Equal(result, expected);
        }

        void GetFile(IEnumerable<Arda.Common.ViewModels.Main.FiscalYearViewModel> models)
        {
            var res = from ma in models
                      orderby ma.FiscalYearID
                      select ma;
                                

            foreach (var m in models)
            {

            }
        }
    }
}
