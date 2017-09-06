using System;
using System.Collections.Generic;
using System.Linq;
using Arda.Kanban.Models;
using Arda.Kanban.Models.Repositories;
using Arda.Kanban.ViewModels;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Arda.Kanban.MongoRepositories
{
    public class UserMongoRepository : IUserRepository
    {
        private MongoContext _context;
        BsonDocument all = new BsonDocument() { };
        BsonDocument byUserName = new BsonDocument() { { nameof(User.name), 1 } };

        public UserMongoRepository(MongoContext context)
        {
            _context = context;
        }


        public bool AddNewUser(UserKanbanViewModel user)
        {
            var model = new User()
            {
                _id = user.UniqueName,
                name = user.Name
            };

            _context.Users.InsertOne(model);

            return true;
        }

        public bool DeleteUserByID(string userID)
        {
            BsonDocument byUserId = new BsonDocument() { { nameof(User._id), userID } };

            _context.Users.DeleteOne(byUserId);

            return true;
        }

        public IEnumerable<UserKanbanViewModel> GetAllUsers()
        {
            var users = (from u in _context.Users.Find(all).Sort(byUserName).ToEnumerable()
                         select new UserKanbanViewModel
                         {
                             UniqueName = u._id,
                             Name = u.name
                         }).ToList();

            return users;
        }
    }
}
