using System;
using System.Collections.Generic;
using System.Linq;
using Arda.Kanban.Models;
using Arda.Kanban.Models.Repositories;
using Arda.Kanban.ViewModels;

using MongoDB.Driver;
using MongoDB.Bson;

namespace Arda.Kanban.MongoRepositories
{
    public class FiscalYearMongoRepository : IFiscalYearRepository
    {
        private MongoContext _context;
        BsonDocument all = new BsonDocument() { };
        BsonDocument byFullNumericFiscalYear = new BsonDocument() { { nameof(FiscalYear.fullNumericFiscalYear), 1 } };

        public FiscalYearMongoRepository(MongoContext context)
        {
            this._context = context;
        }
        
        public bool AddNewFiscalYear(FiscalYearViewModel fiscalyear)
        {
            var fy = new FiscalYear()
            {
                _id = fiscalyear.FiscalYearID.ToString(),
                fullNumericFiscalYear = fiscalyear.FullNumericFiscalYearMain,
                textualFiscalYear = fiscalyear.TextualFiscalYearMain
            };

            _context.FiscalYear.InsertOne(fy);

            return true;
        }
        
        public List<FiscalYearViewModel> GetAllFiscalYears()
        {
            var response = (from f in _context.FiscalYear.Find(all)
                                              .Sort(byFullNumericFiscalYear)
                                              .ToEnumerable()
                            select new FiscalYearViewModel
                            {
                                FiscalYearID = Guid.Parse(f._id),
                                FullNumericFiscalYearMain = f.fullNumericFiscalYear,
                                TextualFiscalYearMain = f.textualFiscalYear
                            }).ToList();

            return response;
        }

        // Return fiscal year based on ID
        public FiscalYearViewModel GetFiscalYearByID(Guid id)
        {
            //var response = _context.FiscalYears.Where(fy => fy.FiscalYearID.Equals(id)).SingleOrDefault();

            //var fiscalYear = new FiscalYearViewModel()
            //{
            //    FiscalYearID = response.FiscalYearID,
            //    TextualFiscalYearMain = response.TextualFiscalYear,
            //    FullNumericFiscalYearMain = response.FullNumericFiscalYear
            //};
            
            throw new NotImplementedException();
        }

        // Update fiscal year data based on ID
        public bool EditFiscalYearByID(FiscalYearViewModel fiscalyear)
        {
            //var fiscalYearToBeUpdated = _context.FiscalYears.SingleOrDefault(fy => fy.FiscalYearID.Equals(fiscalyear.FiscalYearID));

            //if (fiscalYearToBeUpdated != null)
            //{
            //    // Update informations of object
            //    fiscalYearToBeUpdated.FullNumericFiscalYear = fiscalyear.FullNumericFiscalYearMain;
            //    fiscalYearToBeUpdated.TextualFiscalYear = fiscalyear.TextualFiscalYearMain;

            //    var response = _context.SaveChanges();

            //    return true;
            //}

            throw new NotImplementedException();
        }

        // Delete fiscal year based on ID
        public bool DeleteFiscalYearByID(Guid id)
        {
            //var fiscalYearToBeDeleted = _context.FiscalYears.SingleOrDefault(fy => fy.FiscalYearID.Equals(id));


            throw new NotImplementedException();
        }
    }
}
