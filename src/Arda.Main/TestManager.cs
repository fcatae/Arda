using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ArdaSDK.Kanban;
using ArdaSDK.Kanban.Models;

namespace Arda.Main
{
    public class TestManager
    {
        static KanbanClient _client;
        static string _user;

        public static void TestStatic()
        {
            Console.WriteLine("Test");

            _client = Arda.Common.Utils.Util.KanbanClient;

            _user = "fcatae@microsoft.com";

            var test = new TestManager();

            test.TestMain();
        }

        public void TestMain()
        {
            // Add a workload
            object result = _client.WorkspaceFoldersService.AddItem(_user, new AddItemInput() { Title = "Test to be removed", ItemState = 2 });

            // force the Cast
            WorkspaceItem workload = (WorkspaceItem)result;

            // check if not null
            Guid workloadId = workload.Id.Value;

            // Delete the workload
            _client.WorkspaceItemsService.Delete(workloadId);

        }

        public void WorkspaceFolders_AddItems()
        {
            // Add Test with different properties
            _client.WorkspaceFoldersService.AddItem(_user, new AddItemInput() { Title = "Test 1" });
            _client.WorkspaceFoldersService.AddItem(_user, new AddItemInput() { Title = "Test 2", Description = "123" });
            _client.WorkspaceFoldersService.AddItem(_user, new AddItemInput() { Title = "Test 3", ItemState = 2 });
        }

        public void WorkspaceFolders_ListItems()
        {
            string user = "fcatae@microsoft.com";
            string ARDA = "arda";

            // Show the current dashboard
            var dashboardItems = _client.WorkspaceFoldersService.ListItems(user);

            var archivedItems = _client.WorkspaceFoldersService.ListItems(user, type: "archived");

            var backlogItems = _client.WorkspaceFoldersService.ListItems(user, type: "backlog");

            var taggedItems = _client.WorkspaceFoldersService.ListItems(ARDA, type: "tag");
        }
    }
}
