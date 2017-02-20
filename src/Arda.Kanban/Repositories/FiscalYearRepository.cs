using Arda.Common.Interfaces.Kanban;
using Arda.Common.Models.Kanban;
using System;
using System.Collections.Generic;
using System.Linq;
using Arda.Common.ViewModels.Main;
using Arda.Kanban.Models;

namespace Arda.Kanban.Repositories
{
    public class FiscalYearRepository : IFiscalYearRepository
    {
        private KanbanContext _context;

        public FiscalYearRepository(KanbanContext context)
        {
            _context = context;
        }

        // Adds a new fiscal year to the system.
        public bool AddNewFiscalYear(FiscalYearViewModel fiscalyear)
        {
            var fiscalYearToBeSaved = new FiscalYear()
            {
                FiscalYearID = fiscalyear.FiscalYearID,
                FullNumericFiscalYear = fiscalyear.FullNumericFiscalYearMain,
                TextualFiscalYear = fiscalyear.TextualFiscalYearMain
            };

            _context.FiscalYears.Add(fiscalYearToBeSaved);
            var response = _context.SaveChanges();

            if (response > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Return a 'numberOfOccurencies' to controller.
        public List<FiscalYearViewModel> GetAllFiscalYears()
        {
            //_context.FiscalYears.OrderByDescending(fy => fy.FullNumericFiscalYear).ToList();
            var response = (from f in _context.FiscalYears
                            orderby f.FullNumericFiscalYear
                            select new FiscalYearViewModel
                            {
                                FiscalYearID = f.FiscalYearID,
                                FullNumericFiscalYearMain = f.FullNumericFiscalYear,
                                TextualFiscalYearMain = f.TextualFiscalYear
                            }).ToList();

            return response;
        }

        // Return fiscal year based on ID
        public FiscalYearViewModel GetFiscalYearByID(Guid id)
        {
            var response = _context.FiscalYears.Where(fy => fy.FiscalYearID.Equals(id)).SingleOrDefault();

            var fiscalYear = new FiscalYearViewModel()
            {
                FiscalYearID = response.FiscalYearID,
                TextualFiscalYearMain = response.TextualFiscalYear,
                FullNumericFiscalYearMain = response.FullNumericFiscalYear
            };

            return fiscalYear;
        }

        // Update fiscal year data based on ID
        public bool EditFiscalYearByID(FiscalYearViewModel fiscalyear)
        {
            var fiscalYearToBeUpdated = _context.FiscalYears.SingleOrDefault(fy => fy.FiscalYearID.Equals(fiscalyear.FiscalYearID));

            if (fiscalYearToBeUpdated != null)
            {
                // Update informations of object
                fiscalYearToBeUpdated.FullNumericFiscalYear = fiscalyear.FullNumericFiscalYearMain;
                fiscalYearToBeUpdated.TextualFiscalYear = fiscalyear.TextualFiscalYearMain;

                var response = _context.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }

        // Delete fiscal year based on ID
        public bool DeleteFiscalYearByID(Guid id)
        {
            var fiscalYearToBeDeleted = _context.FiscalYears.SingleOrDefault(fy => fy.FiscalYearID.Equals(id));

            if (fiscalYearToBeDeleted != null)
            {
                var response = _context.Remove(fiscalYearToBeDeleted);
                _context.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
