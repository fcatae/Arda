using Arda.Common.ViewModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arda.Common.Interfaces.Kanban
{
    public interface IWorkloadRepository
    {
        // Add a new workload to the database.
        bool AddNewWorkload(WorkloadViewModel workload);

        // Update some workload data based on id.
        bool EditWorkload(WorkloadViewModel workload);

        // Return a list of all workloads.
        IEnumerable<WorkloadViewModel> GetAllWorkloads();

        // Return a specific workload by ID.
        WorkloadViewModel GetWorkloadByID(Guid id);

        // Delete a workload based on ID
        bool DeleteWorkloadByID(Guid id);

        // Get a list of a user's workloads
        IEnumerable<WorkloadsByUserViewModel> GetWorkloadsByUser(string uniqueName);

        // Update the status
        bool UpdateWorkloadStatus(Guid id, int status);

        // Send a notification about new or updated workloads to signed user.
        bool SendNotificationAboutNewOrUpdatedWorkload(string uniqueName, int newOrUpdate);
    }
}
