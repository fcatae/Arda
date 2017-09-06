using System.Collections.Generic;
using System.Linq;
using Arda.Kanban.Models;
using Arda.Kanban.Models.Repositories;
using Arda.Kanban.ViewModels;
using MongoDB.Driver;
using MongoDB.Bson;
using System;

namespace Arda.Kanban.MongoRepositories
{
    public class TechnologyMongoRepository : ITechnologyRepository
    {
        private MongoContext _context;
        BsonDocument all = new BsonDocument() { };
        BsonDocument byTechnologyName = new BsonDocument() { { nameof(Technology.technologyName), 1 } };

        public TechnologyMongoRepository(MongoContext context)
        {
            _context = context;
        }

        public void Create(Guid id, string technologyName)
        {
            var technology = new Technology()
            {
                _id = id.ToString(),
                technologyName = technologyName
            };

            _context.Technologies.InsertOne(technology);
        }

        public IEnumerable<TechnologyViewModel> GetAllTechnologies()
        {
            var response = (from t in _context.Technologies.Find(all).Sort(byTechnologyName).ToEnumerable()
                            select new TechnologyViewModel
                            {
                                TechnologyID = Guid.Parse(t._id),
                                TechnologyName = t.technologyName
                            }).ToList();

            return response;
        }
    }
}
