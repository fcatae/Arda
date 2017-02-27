using System;
using System.Linq;
using Xunit;
using System.Collections.Generic;
using Arda.Kanban.Models;
using Arda.Kanban.Repositories;
using Arda.Common.ViewModels.Kanban;

namespace IntegrationTests
{
    public class User : ISupportSnapshot<UserKanbanViewModel>
    {
        public IEnumerable<UserKanbanViewModel> GetSnapshot(TransactionalKanbanContext context)
        {
            UserRepository user = new UserRepository(context);

            return user.GetAllUsers().ToArray();
        }

        [Fact]
        public void User_GetAllUsers_Should_ReturnAllValues()
        {
            ArdaTestMgr.Validate(this, $"User.GetAllUsers()",
                (list, ctx) => {
                    var rows = from r in list
                               select r.Name;

                    return rows;
                });
        }

        [Fact]
        public void User_AddNewUser_Should_AddRow()
        {
            string USER_UNIQUENAME = "newuser@domain.com";
            string USER_NAME = "New User 1";

            ArdaTestMgr.Validate(this, $"User.AddNewUser({USER_UNIQUENAME},{USER_NAME})",
                (list, ctx) => {
                    UserRepository user = new UserRepository(ctx);

                    var row = new UserKanbanViewModel()
                    {
                        UniqueName = USER_UNIQUENAME,
                        Name = USER_NAME
                    };

                    user.AddNewUser(row);

                    return user.GetAllUsers();
                });
        }

        [Fact]
        public void User_AddNewFiscalYear_Should_AddRow()
        {
            string USER_UNIQUENAME = "guest@ardademo.onmicrosoft.com";

            ArdaTestMgr.Validate(this, $"user.DeleteUserByID({USER_UNIQUENAME})",
                (list, ctx) =>
                {
                    UserRepository user = new UserRepository(ctx);

                    user.DeleteUserByID(USER_UNIQUENAME);

                    return user.GetAllUsers();
                });
        }

    }
}
