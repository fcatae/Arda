using System.Collections.Generic;
using Arda.Kanban.ViewModels;

namespace Arda.Kanban.Models.Repositories
{
    public interface IUserRepository
    {
        // Add a new user to the database.
        bool AddNewUser(UserKanbanViewModel user);

        // Delete a user based on id.
        bool DeleteUserByID(string userID);

        // Return a list of all users.
        IEnumerable<UserKanbanViewModel> GetAllUsers();
    }
}
