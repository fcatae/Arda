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
    }
}
