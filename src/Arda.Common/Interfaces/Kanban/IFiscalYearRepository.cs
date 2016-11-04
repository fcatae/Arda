using Arda.Common.ViewModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arda.Common.Interfaces.Kanban
{
    public interface IFiscalYearRepository
    {
        // Add a new fiscal year to the database.
        bool AddNewFiscalYear(FiscalYearViewModel fiscalyear);

        // Update some fiscal year data based on id.
        bool EditFiscalYearByID(FiscalYearViewModel fiscalyear);

        // Return a list of fiscal years.
        List<FiscalYearViewModel> GetAllFiscalYears();

        // Return a specific fiscal year by ID.
        FiscalYearViewModel GetFiscalYearByID(Guid id);

        // Delete a fiscal year based on ID
        bool DeleteFiscalYearByID(Guid id);
    }
}
