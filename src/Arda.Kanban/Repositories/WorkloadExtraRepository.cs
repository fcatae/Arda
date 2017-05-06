using Arda.Kanban.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Arda.Kanban.Models.Repositories;
using Arda.Kanban.ViewModels;

namespace Arda.Kanban.Repositories
{
    public class WorkloadExtraRepository // : IWorkloadExtraRepository
    {
        KanbanContext _context;

        public WorkloadExtraRepository(KanbanContext context)
        {
            _context = context;
        }

        public void AssignTag(Guid wbid, string tag)
        {
            WorkloadBacklogTags workloadTag = new WorkloadBacklogTags()
            {
                TagId = tag,
                WorkloadBacklogWBID = wbid
            };

            _context.Tags.Add(workloadTag);
            _context.SaveChanges();
        }
        
        public IEnumerable<WorkloadsByUserViewModel> GetWorkloads(string uniqueName)
        {
            var workloads = (from wb in _context.WorkloadBacklogs
                             join tag in _context.Tags on wb.WBID equals tag.WorkloadBacklogWBID

                             // where (int)wb.WBStatus < 3
                             where tag.TagId == uniqueName
                             orderby wb.WBTitle
                             select new WorkloadsByUserViewModel
                             {
                                 _WorkloadID = wb.WBID,
                                 _WorkloadTitle = wb.WBTitle,
                                 _WorkloadStartDate = wb.WBStartDate,
                                 _WorkloadEndDate = wb.WBEndDate,
                                 _WorkloadStatus = (int)wb.WBStatus,
                                 _WorkloadIsWorkload = wb.WBIsWorkload,
                                 _WorkloadAttachments = wb.WBFiles.Count,
                                 _WorkloadExpertise = wb.WBExpertise.ToString(),
                                 _WorkloadUsers = (from wbusers in _context.WorkloadBacklogUsers
                                                   where wbusers.WorkloadBacklog.WBID == wb.WBID
                                                   select new Tuple<string, string>(wbusers.User.UniqueName, wbusers.User.Name)).ToList(),

                                 _WorkloadHours = 0 // ignore

                             }).ToList();


            return workloads;
        }

        public IEnumerable<WorkloadStatusViewModel> GetWorkloadStatus(string tagging)
        {
            var workloads = (from wb in _context.WorkloadBacklogs
                             join tag in _context.Tags on wb.WBID equals tag.WorkloadBacklogWBID
                             
                             // where (int)wb.WBStatus < 3
                             where tag.TagId == tagging
                             orderby wb.WBTitle
                             select new WorkloadStatusViewModel
                             {
                                 WorkloadID = wb.WBID,
                                 Title = wb.WBTitle,
                                 State = (int)wb.WBStatus,
                                 Users = (from u in wb.WBUsers
                                          select u.UserUniqueName).ToArray(),
                                 StatusText = (wb.LastAppointmentId != null) ? (from ap in _context.Appointments
                                               where ap.AppointmentID == wb.LastAppointmentId 
                                               select ap.AppointmentComment).First() : ""

                             }).ToList();


            return workloads;
        }
    }
}
