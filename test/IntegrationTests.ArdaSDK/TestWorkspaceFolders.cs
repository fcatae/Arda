using System;
using System.Linq;
using Xunit;
using Microsoft.Extensions.Configuration;

using ArdaSDK.Kanban;
using ArdaSDK.Kanban.Models;

namespace IntegrationTests.ArdaSDK
{
    public class KanbanFolder
    {
        static KanbanClient _client;
        static string _user = "fcatae@microsoft.com";

        static KanbanFolder()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("microservices.json", true);

            builder.AddUserSecrets("arda-20160816073715");

            var config = builder.Build();

            var kanbanUrl = config["Endpoints:kanban-service"];

            Console.WriteLine("- Kanban URL = " + kanbanUrl);

            _client = new KanbanClient(new Uri(kanbanUrl));
        }

        [Fact]
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

        [Fact]
        public void WorkspaceFolders_AddDelete()
        {
            // Take Before snapshot
            var beforeDashboard = _client.WorkspaceFoldersService.ListItems(_user).ToDictionary(w => w.Id.Value);

            // Add a workload
            WorkspaceItem result = (WorkspaceItem)_client.WorkspaceFoldersService.AddItem(_user, new AddItemInput() { Title = "Test to be removed", ItemState = 2 });
            Guid workloadId = result.Id.Value;

            // Check 1
            if (beforeDashboard.ContainsKey(workloadId))
                throw new InvalidOperationException("workload should not exist before [Add]");

            // Take After snapshot
            var afterDashboard = _client.WorkspaceFoldersService.ListItems(_user).ToDictionary(w => w.Id.Value);

            // Check 2
            if (!afterDashboard.ContainsKey(workloadId))
                throw new InvalidOperationException("workload must exist after [Add]");

            // Delete the workload
            _client.WorkspaceItemsService.Delete(workloadId);

            // Take Final snapshot
            var finalDashboard = _client.WorkspaceFoldersService.ListItems(_user).ToDictionary(w => w.Id.Value);

            // Check 3
            if (beforeDashboard.ContainsKey(workloadId))
                throw new InvalidOperationException("workload should not exist after [Add] + [Delete]");
        }

        [Fact]
        public void WorkspaceItem_EditItem()
        {
            bool success = TestEdit(TestWorkspaceEdit);

            if (!success)
                throw new InvalidOperationException("TestEdit(TestWorkspaceEdit)");
        }

        [Fact]
        public void WorkspaceItem_PatchEdit()
        {
            bool success = TestEdit(TestWorkspacePatchEdit);

            if (!success)
                throw new InvalidOperationException("TestEdit(TestWorkspacePatchEdit)");
        }

        [Fact]
        public void WorkspaceFolders_TestLogs()
        {
            // Add a workload
            WorkspaceItem result = (WorkspaceItem)_client.WorkspaceFoldersService.AddItem(_user, new AddItemInput() { Title = "Test to be removed", ItemState = 2 });
            Guid workloadId = result.Id.Value;

            var initial = _client.WorkspaceItemLogsService.GetLogs(workloadId);

            _client.WorkspaceItemLogsService.AppendLog(workloadId, new InputAppendLog() { Text = "Example" }, _user);

            var final = _client.WorkspaceItemLogsService.GetLogs(workloadId);
            if (final.Count() != 1)
                throw new InvalidOperationException();

            // Delete the workload
            _client.WorkspaceItemsService.Delete(workloadId);
        }


        [Fact]
        public void WorkspaceItem_UpdateStatus()
        {
            bool success = TestEdit(TestWorkspaceUpdateStatus);

            if (!success)
                throw new InvalidOperationException("TestEdit(TestWorkspaceUpdateStatus)");
        }

        bool TestEdit(Func<WorkspaceItem, bool> testEditFunc)
        {
            var item = new AddItemInput() { Title = "Test Edit", ItemState = 0 };
            bool success = false;

            // Add 
            WorkspaceItem workload = (WorkspaceItem)_client.WorkspaceFoldersService.AddItem(_user, item);
            Guid workloadId = workload.Id.Value;

            try
            {
                success = testEditFunc(workload);
            }
            finally
            {
                _client.WorkspaceItemsService.Delete(workloadId);
            }

            return success;
        }

        bool TestWorkspaceEdit(WorkspaceItem workload)
        {
            string START = "!";
            string DESCRIPTION1 = "First Description";

            Guid workloadId = workload.Id.Value;

            workload.Title = DESCRIPTION1;
            workload.Description = START + DESCRIPTION1;
            workload.ItemState = 1;

            // Edit
            _client.WorkspaceItemsService.Edit(workload);

            var result1 = _client.WorkspaceItemsService.GetItem(workloadId);

            return (result1.Title == DESCRIPTION1) &&
                    (result1.Description == START + DESCRIPTION1) &&
                    (result1.ItemState == 1);
        }

        bool TestWorkspacePatchEdit(WorkspaceItem workload)
        {
            string START = "!";
            string DESCRIPTION2 = "Multiple Descriptions";

            Guid workloadId = workload.Id.Value;

            // PatchEdit 
            _client.WorkspaceItemsService.EditItem(workloadId, new EditItemInput() { Title = DESCRIPTION2 });
            _client.WorkspaceItemsService.EditItem(workloadId, new EditItemInput() { Description = START + DESCRIPTION2 });
            _client.WorkspaceItemsService.EditItem(workloadId, new EditItemInput() { ItemState = 2 });

            // Check
            var result1 = _client.WorkspaceItemsService.GetItem(workloadId);

            return (result1.Title == DESCRIPTION2) &&
                    (result1.Description == START + DESCRIPTION2) &&
                    (result1.ItemState == 2);
        }

        bool TestWorkspaceUpdateStatus(WorkspaceItem workload)
        {
            Guid workloadId = workload.Id.Value;

            var wis = _client.WorkspaceItemsService;

            for (int n = 0; n < 3; n++)
            {
                // Update Status to 'n'
                wis.UpdateStatus(workloadId, n);

                // Check failure
                if (wis.GetItem(workloadId).ItemState != n)
                    return false;
            }

            return true;
        }
    }
}
