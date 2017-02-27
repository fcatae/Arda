using System;
using System.Linq;
using Xunit;
using System.Collections.Generic;
using Arda.Kanban.Models;
using Arda.Kanban.Repositories;
using Arda.Common.ViewModels.Main;

namespace IntegrationTests
{
    public class Workload : ISupportSnapshot<WorkloadViewModel>
    {
        public IEnumerable<WorkloadViewModel> GetSnapshot(TransactionalKanbanContext context)
        {
            WorkloadRepository workload = new WorkloadRepository(context);

            return workload.GetAllWorkloads().ToArray();
        }

        [Fact]
        public void Workload_GetAllWorkloads_Should_ReturnAllValues() 
        {
            ArdaTestMgr.Validate(this, $"Workload.GetAllWorkloads()",
                (list, ctx) => {
                    var rows = from r in list
                               select r.WBTitle;

                    return rows;
                });
        }

        [Fact]
        public void Workload_AddNewWorkload_Should_AddRow()
        {
            ArdaTestMgr.Validate(this, $"Workload.AddNewWorkload()",
                (list, ctx) => {
                    WorkloadRepository workload = new WorkloadRepository(ctx);

                    var rows = from r in list
                               select r.WBTitle;

                    var row = new WorkloadViewModel()
                    {
                        // TODO: fill
                    };

                    workload.AddNewWorkload(row);

                    return rows;
                });
        }
        
        [Fact]
        public void Workload_DeleteWorkloadByID_Should_ReturnRemoveExactlyOne()
        {
            string GUID = "{...}";

            ArdaTestMgr.Validate(this, $"Workload.DeleteWorkloadByID({GUID})",
                (list, ctx) => {
                    WorkloadRepository workload = new WorkloadRepository(ctx);
                    
                    workload.DeleteWorkloadByID(Guid.Parse(GUID));

                    return workload.GetAllWorkloads();
                });
        }

        [Fact]
        public void Workload_GetWorkloadByID_Should_ReturnExactlyOne()
        {
            string GUID = "{...}";

            ArdaTestMgr.Validate(this, $"Workload.GetWorkloadByID({GUID})",
                (list, ctx) => {
                    WorkloadRepository workload = new WorkloadRepository(ctx);

                    var row = workload.GetWorkloadByID(Guid.Parse(GUID));

                    return row;
                });
        }

        [Fact]
        public void Workload_GetWorkloadsByUser_Should_ReturnUserData()
        {
            string USER_UNIQUENAME = "admin@ardademo.onmicrosoft.com";

            ArdaTestMgr.Validate(this, $"Workload.GetWorkloadsByUser({USER_UNIQUENAME})",
                (list, ctx) => {
                    WorkloadRepository workload = new WorkloadRepository(ctx);

                    var rows = workload.GetWorkloadsByUser(USER_UNIQUENAME);

                    return rows;
                });
        }

        [Fact]
        public void Workload_EditWorkload_Should_ChangeRow()
        {
            string GUID = "{...}";

            ArdaTestMgr.Validate(this, $"Workload.EditWorkload({GUID})",
                (list, ctx) => {
                    WorkloadRepository workload = new WorkloadRepository(ctx);

                    var row = (from r in list
                               where r.WBID == Guid.Parse(GUID)
                               select r).First();

                    // edit

                    workload.EditWorkload(row);

                    return workload.GetAllWorkloads();
                });
        }        

        [Fact]
        public void Workload_UpdateWorkloadStatus_Should_UpdateOneStatus()
        {
            string GUID = "{...}";
            int STATUS = 4;

            ArdaTestMgr.Validate(this, $"Workload.UpdateWorkloadStatus({GUID},{STATUS})",
                (list, ctx) => {
                    WorkloadRepository workload = new WorkloadRepository(ctx);

                    var row = workload.UpdateWorkloadStatus(Guid.Parse(GUID), STATUS);

                    return row;
                });
        }


    }
}
