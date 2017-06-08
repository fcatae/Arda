using System;
using System.Collections.Generic;
using System.Linq;
using Arda.Kanban.Models;
using Arda.Kanban.Models.Repositories;
using Arda.Kanban.ViewModels;

namespace Arda.Kanban.Repositories
{
    public class UserRepository : IUserRepository
    {
        private KanbanContext _context;

        public UserRepository(KanbanContext context)
        {
            _context = context;
        }


        public bool AddNewUser(UserKanbanViewModel user)
        {
            _context.Users.Add(new User()
            {
                UniqueName = user.UniqueName,
                Name = user.Name
            });
            _context.SaveChanges();

            return true;
        }

        public bool DeleteUserByID(string userID)
        {
            var user = _context.Users.First(u => u.UniqueName == userID);
            //TODO: Remove Appointments and WBs

            //Removing User:
            _context.Users.Remove(user);

            _context.SaveChanges();

            return true;
        }

        public IEnumerable<UserKanbanViewModel> GetAllUsers()
        {
            var response = (from u in _context.Users
                            orderby u.Name
                            select new UserKanbanViewModel
                            {
                                UniqueName = u.UniqueName,
                                Name = u.Name
                            }).ToList();

            return response;
        }
    }
}
