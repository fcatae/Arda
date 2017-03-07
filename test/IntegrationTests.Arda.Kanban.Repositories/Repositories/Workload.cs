using System;
using System.Linq;
using Xunit;
using System.Collections.Generic;
using Arda.Kanban.Models;
using Arda.Kanban.Repositories;
using Arda.Kanban.ViewModels;

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
        public void Workload_GetAllWorkloads() 
        {
            ArdaTestMgr.Validate(this, $"Workload.GetAllWorkloads()",
                (list, ctx) => {
                    var rows = from r in list
                               select r.WBTitle;

                    return rows;
                });
        }

        [Fact]
        public void Workload_AddNewWorkload()
        {
            WorkloadViewModel WORKLOAD1 = new WorkloadViewModel()
            {
                WBActivity = Guid.Parse("1f265df5-adbe-4b7b-a05a-451af058c482"), // POC
                WBComplexity = 1,
                WBCreatedBy = "admin@ardademo.onmicrosoft.com",
                WBCreatedDate = DateTime.Parse("2021-01-20"),
                WBEndDate = DateTime.Parse("2021-01-30"),
                WBDescription = "My Workload Description",
                WBExpertise = 2,
                WBFilesList = null,
                WBID = Guid.Parse("aaaa0022-FD15-428C-9B24-14E6467977AD"),
                WBIsWorkload = true,
                WBMetrics = new Guid[] {
                    Guid.Parse("6da887cb-9edd-42cb-87c9-83ac772d9b65"), // Community
                    Guid.Parse("45979112-aff6-4bfa-878b-02baa8fd1074")  // Education
                },
                WBStartDate = DateTime.Parse("2021-01-25"),
                WBStatus = 3,
                WBTechnologies = new Guid[] {
                    Guid.Parse("9c263d44-2c11-48cd-b876-5ebb540bbf51"), // Infra
                    Guid.Parse("af5d8796-0ca2-4d54-84f7-d3194f5f2426")  // Web & Mobile
                },
                WBTitle = "My Initial Workload",
                WBUsers = new string[] {
                    "user@ardademo.onmicrosoft.com", "admin@ardademo.onmicrosoft.com"
                }
            };

            ArdaTestMgr.Validate(this, $"Workload.AddNewWorkload({WORKLOAD1.WBID})",
                (list, ctx) => {
                    WorkloadRepository workload = new WorkloadRepository(ctx);
                    
                    workload.AddNewWorkload(WORKLOAD1);

                    return workload.GetAllWorkloads();
                });
        }
        
        [Fact]
        public void Workload_DeleteWorkloadByID()
        {
            string GUID = "{C1507019-1D97-4629-8015-C01BF02CE6AB}";

            ArdaTestMgr.Validate(this, $"Workload.DeleteWorkloadByID({GUID})",
                (list, ctx) => {
                    WorkloadRepository workload = new WorkloadRepository(ctx);
                    
                    workload.DeleteWorkloadByID(Guid.Parse(GUID));

                    return workload.GetAllWorkloads();
                });
        }

        [Fact]
        public void Workload_GetWorkloadByID()
        {
            string GUID = "{C1507019-1D97-4629-8015-C01BF02CE6AB}";

            ArdaTestMgr.Validate(this, $"Workload.GetWorkloadByID({GUID})",
                (list, ctx) => {
                    WorkloadRepository workload = new WorkloadRepository(ctx);

                    var row = workload.GetWorkloadByID(Guid.Parse(GUID));

                    return row;
                });
        }

        [Fact]
        public void Workload_GetWorkloadsByUser()
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
        public void Workload_GetWorkloadsByUser_DoesNot_ReturnUseCreatedByField()
        {
            string USER_UNIQUENAME = "user@ardademo.onmicrosoft.com";

            ArdaTestMgr.Validate(this, $"Workload.GetWorkloadsByUser({USER_UNIQUENAME})",
                (list, ctx) => {
                    WorkloadRepository workload = new WorkloadRepository(ctx);

                    var rows = workload.GetWorkloadsByUser(USER_UNIQUENAME);

                    return rows;
                });
        }

        [Fact]
        public void Workload_EditWorkload()
        {
            string GUID = "{90cac674-18c0-4139-8aae-f9711bd2d5f4}";
            
            WorkloadViewModel WORKLOAD1 = new WorkloadViewModel()
            {
                WBActivity = Guid.Parse("1f265df5-adbe-4b7b-a05a-451af058c482"), // POC
                WBComplexity = 1,
                WBCreatedBy = "admin@ardademo.onmicrosoft.com",
                WBCreatedDate = DateTime.Parse("2021-01-20"),
                WBEndDate = DateTime.Parse("2021-01-30"),
                WBDescription = "My Workload Description",
                WBExpertise = 2,
                WBFilesList = null,
                WBID = Guid.Parse("aaaa0022-FD15-428C-9B24-14E6467977AD"),
                WBIsWorkload = true,
                WBMetrics = new Guid[] {
                    Guid.Parse("6da887cb-9edd-42cb-87c9-83ac772d9b65"), // Community
                    Guid.Parse("45979112-aff6-4bfa-878b-02baa8fd1074")  // Education
                },
                WBStartDate = DateTime.Parse("2021-01-25"),
                WBStatus = 3,
                WBTechnologies = new Guid[] {
                    Guid.Parse("9c263d44-2c11-48cd-b876-5ebb540bbf51"), // Infra
                    Guid.Parse("af5d8796-0ca2-4d54-84f7-d3194f5f2426")  // Web & Mobile
                },
                WBTitle = "My Initial Workload",
                WBUsers = new string[] {
                    "user@ardademo.onmicrosoft.com", "admin@ardademo.onmicrosoft.com"
                }
            };

            ArdaTestMgr.Validate(this, $"Workload.EditWorkload({GUID})",
                (list, ctx) => {
                    WorkloadRepository workload = new WorkloadRepository(ctx);

                    var row = WORKLOAD1;

                    row.WBID = Guid.Parse(GUID);

                    workload.EditWorkload(row);

                    return workload.GetAllWorkloads();
                });
        }

        [Fact]
        public void Workload_EditWorkload_Should_NotFailWithEmptyMetrics()
        {
            string GUID = "{C1507019-1D97-4629-8015-C01BF02CE6AB}";

            WorkloadViewModel WORKLOAD1 = new WorkloadViewModel()
            {
                WBActivity = Guid.Parse("1f265df5-adbe-4b7b-a05a-451af058c482"), // POC
                WBComplexity = 1,
                WBCreatedBy = "admin@ardademo.onmicrosoft.com",
                WBCreatedDate = DateTime.Parse("2021-01-20"),
                WBEndDate = DateTime.Parse("2021-01-30"),
                WBDescription = "My Workload Description",
                WBExpertise = 2,
                WBFilesList = null,
                WBID = Guid.Parse("aaaa0022-FD15-428C-9B24-14E6467977AD"),
                WBIsWorkload = true,
                WBMetrics = null,
                WBStartDate = DateTime.Parse("2021-01-25"),
                WBStatus = 3,
                WBTechnologies = null,
                WBTitle = "My Initial Workload",
                WBUsers = null
            };

            ArdaTestMgr.Validate(this, $"Workload.EditWorkload({GUID})",
                (list, ctx) => {
                    WorkloadRepository workload = new WorkloadRepository(ctx);

                    var row = WORKLOAD1;

                    row.WBID = Guid.Parse(GUID);

                    workload.EditWorkload(row);

                    return workload.GetAllWorkloads();
                });
        }
        [Fact]
        public void Workload_UpdateWorkloadStatus()
        {
            string GUID = "{C1507019-1D97-4629-8015-C01BF02CE6AB}";
            int STATUS = 4;

            ArdaTestMgr.Validate(this, $"Workload.UpdateWorkloadStatus({GUID},{STATUS})",
                (list, ctx) => {
                    WorkloadRepository workload = new WorkloadRepository(ctx);

                    workload.UpdateWorkloadStatus(Guid.Parse(GUID), STATUS);

                    return workload.GetAllWorkloads();
                });
        }


    }
}
