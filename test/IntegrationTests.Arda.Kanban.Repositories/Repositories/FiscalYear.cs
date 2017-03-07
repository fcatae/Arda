using System;
using System.Linq;
using Xunit;
using System.Collections.Generic;
using Arda.Kanban.Models;
using Arda.Kanban.Repositories;
using Arda.Kanban.ViewModels;

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
        public void FiscalYear_GetAllFiscalYears() 
        {
            ArdaTestMgr.Validate(this, $"FiscalYear.GetAllFiscalYears()",
                (list, ctx) => {
                    var rows = from r in list
                               select r.FullNumericFiscalYearMain;

                    return rows;
                });
        }

        [Fact]
        public void FiscalYear_GetFiscalYearByID()
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
        public void FiscalYear_AddNewFiscalYear()
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
        public void FiscalYear_EditFiscalYearByID()
        {
            string GUID = "{d38759ab-e310-46f0-a6c3-b0594c2531ab}";
            int YEAR = 2021;
            string YEARTXT = "TEST-2021";

            ArdaTestMgr.Validate(this, $"FiscalYear.EditFiscalYearByID({GUID},{YEAR},{YEARTXT})",
                (list, ctx) => {
                    FiscalYearRepository fiscalYear = new FiscalYearRepository(ctx);

                    var row = list[0];

                    row.FiscalYearID = Guid.Parse(GUID);
                    row.FullNumericFiscalYearMain = YEAR;
                    row.TextualFiscalYearMain = YEARTXT;

                    fiscalYear.EditFiscalYearByID(row);

                    return fiscalYear.GetAllFiscalYears();
                });            

        }



        [Fact]
        public void FiscalYear_DeleteFiscalYearByID()
        {
            int YEAR = 2018;

            ArdaTestMgr.Validate(this, $"FiscalYear.DeleteFiscalYearByID({YEAR})",
                (list, ctx) => {
                    FiscalYearRepository fiscalYear = new FiscalYearRepository(ctx);

                    var fiscalYearId = (from row in list
                                        where row.FullNumericFiscalYearMain == YEAR
                                        select row.FiscalYearID).First();

                    fiscalYear.DeleteFiscalYearByID(fiscalYearId);

                    return fiscalYear.GetAllFiscalYears();
                });            
            }
    }
}
