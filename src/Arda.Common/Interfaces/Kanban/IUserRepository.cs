using Arda.Common.ViewModels.Kanban;
using System.Collections.Generic;

namespace Arda.Common.Interfaces.Kanban
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
