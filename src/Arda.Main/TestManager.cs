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

            test.WorkspaceItem_EditItem();
            test.WorkspaceItem_PatchEdit();
            test.WorkspaceItem_UpdateStatus();

        }

        public void TestMain()
        {
        }

        void WorkspaceItem_EditItem()
        {
            bool success = TestEdit(TestWorkspaceEdit);

            if (!success)
                throw new InvalidOperationException("TestEdit(TestWorkspaceEdit)");
        }

        void WorkspaceItem_PatchEdit()
        {
            bool success = TestEdit(TestWorkspacePatchEdit);

            if (!success)
                throw new InvalidOperationException("TestEdit(TestWorkspacePatchEdit)");
        }

        void WorkspaceItem_UpdateStatus()
        {
            bool success = TestEdit(TestWorkspaceUpdateStatus);

            if (!success)
                throw new InvalidOperationException("TestEdit(TestWorkspaceUpdateStatus)");
        }

        bool TestEdit(Func<WorkspaceItem,bool> testEditFunc)
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

            return  (result1.Title == DESCRIPTION1) &&
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

            return  (result1.Title == DESCRIPTION2) &&
                    (result1.Description == START + DESCRIPTION2) &&
                    (result1.ItemState == 2);           
        }

        bool TestWorkspaceUpdateStatus(WorkspaceItem workload)
        {
            Guid workloadId = workload.Id.Value;

            var wis = _client.WorkspaceItemsService;

            for(int n=0; n<3; n++)
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
