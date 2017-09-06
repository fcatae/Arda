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

            _context.FiscalYears.InsertOne(fy);

            return true;
        }
        
        public List<FiscalYearViewModel> GetAllFiscalYears()
        {
            var response = (from f in _context.FiscalYears.Find(all)
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
            var byId = new BsonDocument { { nameof(FiscalYear._id), id.ToString() } };

            var fiscalYear = _context.FiscalYears.Find(byId).FirstOrDefault();

            return new FiscalYearViewModel()
            {
                FiscalYearID = Guid.Parse(fiscalYear._id),
                TextualFiscalYearMain = fiscalYear.textualFiscalYear,
                FullNumericFiscalYearMain = fiscalYear.fullNumericFiscalYear
            };
        }

        public bool EditFiscalYearByID(FiscalYearViewModel fiscalyear)
        {
            var byID = new BsonDocument { { nameof(FiscalYear._id), fiscalyear.FiscalYearID.ToString() } };
            var updateColumns = Builders<FiscalYear>.Update
                                    .Set(nameof(FiscalYear.fullNumericFiscalYear), fiscalyear.FullNumericFiscalYearMain)
                                    .Set(nameof(FiscalYear.textualFiscalYear), fiscalyear.TextualFiscalYearMain);

            var fiscalYear = _context.FiscalYears.UpdateOne(byID, updateColumns);

            return true;
        }
        
        public bool DeleteFiscalYearByID(Guid id)
        {
            var byId = new BsonDocument { { nameof(FiscalYear._id), id.ToString() } };

            var fiscalYear = _context.FiscalYears.DeleteOne(byId);

            return true;
        }
    }
}
