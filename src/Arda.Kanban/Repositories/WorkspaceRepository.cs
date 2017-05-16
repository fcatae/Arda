using Arda.Kanban.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Arda.Kanban.Models.Repositories;
using Arda.Kanban.ViewModels;

namespace Arda.Kanban.Repositories
{
    public class WorkspaceRepository : IWorkspaceRepository
    {
        KanbanContext _context;

        public WorkspaceRepository(KanbanContext context)
        {
            _context = context;
        }

        public void Create(WorkspaceItem item)
        {
            if (item.Id == Guid.Empty)
                throw new ArgumentNullException(nameof(item.Id));

            // default values
            // Guid defaultId = (item.Id != Guid.Empty) ? item.Id : Guid.NewGuid();
            DateTime startDate = item.StartDate ?? DateTime.Today;
            DateTime endDate = item.EndDate ?? DateTime.Today;
            WorkloadBacklogUser wbUser = new WorkloadBacklogUser() { UserUniqueName = item.CreatedBy };

            var workloadToBeSaved = new WorkloadBacklog()
            {
                WBID = item.Id,
                WBTitle = item.Title,
                WBStatus = (Status)item.ItemState,
                WBDescription = item.Description,

                // metadata
                WBCreatedBy = item.CreatedBy,
                WBCreatedDate = item.CreatedDate,

                // use default values
                WBStartDate = startDate,
                WBEndDate = endDate,
                WBUsers = new[] { wbUser },
                WBIsWorkload = true, // must be true

                // optional values
                WBActivity = null,
                WBAppointments = null,
                WBComplexity = 0,
                WBExpertise = 0,
                WBFiles = null,
                WBMetrics = null,
                WBTechnologies = null
            };

            _context.WorkloadBacklogs.Add(workloadToBeSaved);
            _context.SaveChanges();
        }
        
        public IEnumerable<WorkspaceItem> List()
        {
            var workloads = (from w in _context.WorkloadBacklogs
                             where (int)w.WBStatus < 3
                             select new WorkspaceItem()
                             {
                                 Description = w.WBDescription,
                                 EndDate = w.WBEndDate,
                                 StartDate = w.WBStartDate,
                                 ItemState = (int)w.WBStatus,
                                 Id = w.WBID,
                                 Summary = "",
                                 Title = w.WBTitle,
                                 CreatedBy = w.WBCreatedBy,
                                 CreatedDate = w.WBCreatedDate
                             }).ToArray();

            return workloads;
        }

        public IEnumerable<WorkspaceItem> ListByUser(string uniqueName)
        {
            if (uniqueName == null)
                throw new ArgumentNullException("uniqueName");

            var workloads = (from wbu in _context.WorkloadBacklogUsers
                             join w in _context.WorkloadBacklogs on wbu.WorkloadBacklogWBID equals w.WBID
                             where wbu.UserUniqueName == uniqueName
                             where (int)w.WBStatus < 3
                             select new WorkspaceItem
                             {
                                 Description = w.WBDescription,
                                 EndDate = w.WBEndDate,
                                 StartDate = w.WBStartDate,
                                 ItemState = (int)w.WBStatus,
                                 Id = w.WBID,
                                 Summary = "",
                                 Title = w.WBTitle,
                                 CreatedBy = w.WBCreatedBy,
                                 CreatedDate = w.WBCreatedDate
                             }).ToArray();

            return workloads;
        }
        
        public IEnumerable<WorkspaceItem> ListByTag(string tag)
        {
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));

            var workloads = (from w in _context.WorkloadBacklogs
                             join t in _context.Tags on w.WBID equals t.WorkloadBacklogWBID
                             where t.TagId == tag
                             // where (int)w.WBStatus < 3 // ignore status
                             select new WorkspaceItem
                             {
                                 Description = w.WBDescription,
                                 EndDate = w.WBEndDate,
                                 StartDate = w.WBStartDate,
                                 ItemState = (int)w.WBStatus,
                                 Id = w.WBID,
                                 Summary = "",
                                 Title = w.WBTitle,
                                 CreatedBy = w.WBCreatedBy,
                                 CreatedDate = w.WBCreatedDate
                             }).ToArray();

            return workloads;
        }

        public WorkspaceItem Get(Guid itemId)
        {
            var workload = TryGet(itemId);

            if (workload == null)
                throw new InvalidOperationException("WorkspaceItem.Get() returned (null)");

            return workload;
        }

        public WorkspaceItem TryGet(Guid itemId)
        {
            if (itemId == Guid.Empty)
                throw new ArgumentNullException(nameof(itemId));

            var workload = (from w in _context.WorkloadBacklogs
                             where w.WBID == itemId
                             select new WorkspaceItem()
                             {
                                 Description = w.WBDescription,
                                 EndDate = w.WBEndDate,
                                 StartDate = w.WBStartDate,
                                 ItemState = (int)w.WBStatus,
                                 Id = w.WBID,
                                 Summary = "",
                                 Title = w.WBTitle,
                                 CreatedBy = w.WBCreatedBy,
                                 CreatedDate = w.WBCreatedDate
                             }).FirstOrDefault();            

            return workload;
        }

        public void SetStatus(Guid itemId, int status)
        {
            if (itemId == Guid.Empty)
                throw new ArgumentNullException(nameof(itemId));

            var workload = (from w in _context.WorkloadBacklogs
                            where w.WBID == itemId
                            select w).FirstOrDefault();

            if (workload == null)
                throw new InvalidOperationException("WorkspaceItem.Get() returned (null)");

            workload.WBStatus = (Status)status;

            _context.SaveChanges();            
        }

        public IEnumerable<WorkspaceItem> ListArchivedByUser(string uniqueName)
        {
            if (uniqueName == null)
                throw new ArgumentNullException("uniqueName");

            var workloads = (from wbu in _context.WorkloadBacklogUsers
                             join w in _context.WorkloadBacklogs on wbu.WorkloadBacklogWBID equals w.WBID
                             where wbu.UserUniqueName == uniqueName
                             where (int)w.WBStatus >= 3         // archived items
                             orderby w.WBCreatedDate descending // ORDER BY descending
                             select new WorkspaceItem
                             {
                                 Description = w.WBDescription,
                                 EndDate = w.WBEndDate,
                                 StartDate = w.WBStartDate,
                                 ItemState = (int)w.WBStatus,
                                 Id = w.WBID,
                                 Summary = "",
                                 Title = w.WBTitle,
                                 CreatedBy = w.WBCreatedBy,
                                 CreatedDate = w.WBCreatedDate
                             }).ToArray();

            return workloads;
        }

        public IEnumerable<WorkspaceItem> ListBacklogByUser(string uniqueName)
        {
            if (uniqueName == null)
                throw new ArgumentNullException("uniqueName");

            var workloads = (from wbu in _context.WorkloadBacklogUsers
                             join w in _context.WorkloadBacklogs on wbu.WorkloadBacklogWBID equals w.WBID
                             where wbu.UserUniqueName == uniqueName
                             where w.WBIsWorkload == false      // backlog items
                             orderby w.WBCreatedDate descending // ORDER BY descending
                             select new WorkspaceItem
                             {
                                 Description = w.WBDescription,
                                 EndDate = w.WBEndDate,
                                 StartDate = w.WBStartDate,
                                 ItemState = (int)w.WBStatus,
                                 Id = w.WBID,
                                 Summary = "",
                                 Title = w.WBTitle,
                                 CreatedBy = w.WBCreatedBy,
                                 CreatedDate = w.WBCreatedDate
                             }).ToArray();

            return workloads;
        }

        public bool Edit(WorkspaceItem workload)
        {
            Guid itemId = workload.Id;

            if (itemId == Guid.Empty)
                throw new ArgumentNullException(nameof(itemId));

            var workloadToBeUpdated = _context.WorkloadBacklogs.FirstOrDefault(w => w.WBID == itemId);

            if (workloadToBeUpdated == null)
            {
                return false;
            }
                
            workloadToBeUpdated.WBTitle = workload.Title;
            workloadToBeUpdated.WBDescription = workload.Description;
            workloadToBeUpdated.WBStatus = (Status)workload.ItemState;

            if (workload.StartDate != null)
            {
                workloadToBeUpdated.WBStartDate = workload.StartDate.Value;
            }

            if (workload.EndDate != null)
            {
                workloadToBeUpdated.WBEndDate = workload.EndDate.Value;
            }

            // Do nothing to Summary (yet)
            // workload.Summary

            // Do not update WBCreatedBy + WBCreatedDate
            
            _context.SaveChanges();

            return true;
        }

        public void Delete(Guid itemId)
        {
            if (itemId == Guid.Empty)
                throw new ArgumentNullException(nameof(itemId));

            var workload = _context.WorkloadBacklogs.FirstOrDefault(w => w.WBID == itemId);

            _context.WorkloadBacklogs.Remove(workload);
            _context.SaveChanges();            
        }

        public bool EditWorkload(WorkloadViewModel workload)
        {
            var checkWBTechnologies = (workload.WBTechnologies == null) ? 0 : workload.WBTechnologies.Count();
            var checkWBUsers = (workload.WBUsers == null) ? 0 : workload.WBUsers.Count();
            var checkWBMetrics = (workload.WBMetrics == null) ? 0 : workload.WBMetrics.Count();

            //Get files:
            var files = _context.Files.Where(f => f.WorkloadBacklog.WBID == workload.WBID);
            //Load Metrics:
            var metrics = _context.WorkloadBacklogMetrics.Where(wbm => wbm.WorkloadBacklog.WBID == workload.WBID);
            //Load Technologies:
            var technologies = _context.WorkloadBacklogTechnologies.Where(wbt => wbt.WorkloadBacklog.WBID == workload.WBID);
            //Load Users:
            var users = _context.WorkloadBacklogUsers.Where(wbu => wbu.WorkloadBacklog.WBID == workload.WBID);
            //Remove Everything:
            _context.Files.RemoveRange(files);
            _context.WorkloadBacklogMetrics.RemoveRange(metrics);
            _context.WorkloadBacklogTechnologies.RemoveRange(technologies);
            _context.WorkloadBacklogUsers.RemoveRange(users);
            _context.SaveChanges();

            // CHECK
            var assertWBTechnologies = (workload.WBTechnologies == null) ? 0 : workload.WBTechnologies.Count();
            var assertWBUsers = (workload.WBUsers == null) ? 0 : workload.WBUsers.Count();
            var assertWBMetrics = (workload.WBMetrics == null) ? 0 : workload.WBMetrics.Count();

            if (checkWBTechnologies != assertWBTechnologies)
                throw new InvalidOperationException("Bug #44: collections changed");

            if (checkWBUsers != assertWBUsers)
                throw new InvalidOperationException("Bug #44: collections changed");

            if (checkWBMetrics != assertWBMetrics)
                throw new InvalidOperationException("Bug #44: collections changed");

            //Load workload from DB:
            var workloadToBeUpdated = _context.WorkloadBacklogs.First(w => w.WBID == workload.WBID);
            //Load related Activity:
            var activity = _context.Activities.First(a => a.ActivityID == workload.WBActivity);
            //Load related Metrics:
            var metricList = new List<WorkloadBacklogMetric>();

            if (workload.WBMetrics != null)
            {
                foreach (var mId in workload.WBMetrics)
                {
                    var metric = _context.Metrics.First(m => m.MetricID == mId);
                    metricList.Add(new WorkloadBacklogMetric()
                    {
                        Metric = metric
                    });
                }
            }
            
            //Load related Technologies:
            var technologyList = new List<WorkloadBacklogTechnology>();

            if ( workload.WBTechnologies != null )
            {
                foreach (var tId in workload.WBTechnologies)
                {
                    var technology = _context.Technologies.First(t => t.TechnologyID == tId);
                    technologyList.Add(new WorkloadBacklogTechnology()
                    {
                        Technology = technology
                    });
                }
            }

            //Load related Users:
            var userList = new List<WorkloadBacklogUser>();
            if( workload.WBUsers != null )
            {
                foreach (var uniqueName in workload.WBUsers)
                {
                    var user = _context.Users.First(u => u.UniqueName == uniqueName);
                    userList.Add(new WorkloadBacklogUser()
                    {
                        User = user
                    });
                }
            }
            //Associate related Files:
            var filesList = new List<File>();

            if (workload.WBFilesList != null)
            {
                foreach (var f in workload.WBFilesList)
                {
                    filesList.Add(new File()
                    {
                        FileID = f.Item1,
                        FileLink = f.Item2,
                        FileName = f.Item3,
                        FileDescription = string.Empty,
                    });
                }
            }

            //Update workload object:
            workloadToBeUpdated.WBActivity = activity;
            //workloadToBeUpdated.WBAppointments = null;
            workloadToBeUpdated.WBComplexity = (Complexity)workload.WBComplexity;
            //workloadToBeUpdated.WBCreatedBy = workload.WBCreatedBy;
            //workloadToBeUpdated.WBCreatedDate = workload.WBCreatedDate;
            workloadToBeUpdated.WBDescription = workload.WBDescription;
            workloadToBeUpdated.WBEndDate = workload.WBEndDate;
            workloadToBeUpdated.WBExpertise = (Expertise)workload.WBExpertise;
            workloadToBeUpdated.WBFiles = filesList;
            workloadToBeUpdated.WBID = workload.WBID;
            workloadToBeUpdated.WBIsWorkload = workload.WBIsWorkload;
            workloadToBeUpdated.WBMetrics = metricList;
            workloadToBeUpdated.WBStartDate = workload.WBStartDate;
            workloadToBeUpdated.WBStatus = (Status)workload.WBStatus;
            workloadToBeUpdated.WBTechnologies = technologyList;
            workloadToBeUpdated.WBTitle = workload.WBTitle;
            workloadToBeUpdated.WBUsers = userList;

            _context.SaveChanges();
            return true;
        }

        // appointments
        
        public List<WorkspaceItemLog> GetLogs(Guid wbid)
        {
            var response = (from a in _context.Appointments
                            where a.AppointmentWorkloadWBID == wbid
                            orderby a.AppointmentDate descending
                            select new WorkspaceItemLog
                            {
                                // _AppointmentWorkloadWBID and _WorkloadTitle: does not return the workload title -- it is obvious from wbid
                                Id = a.AppointmentID,
                                CreatedDate = a.AppointmentDate,
                                CreatedBy = a.AppointmentUserUniqueName,
                                Text= a.AppointmentComment,

                                Properties = new WorkspaceItemLogProperties()
                                {
                                    LaborHours = a.AppointmentHoursDispensed,
                                    Expenses = a.AppointmentTE
                                }
                            }).ToList();

            return response;
        }

        public void AppendLog(Guid workloadId, WorkspaceItemLog log)
        {
            if (workloadId == Guid.Empty)
                throw new ArgumentNullException(nameof(workloadId));

            if (log.Id == Guid.Empty)
                throw new ArgumentNullException(nameof(log.Id));

            if (log.CreatedBy == null)
                throw new ArgumentNullException(nameof(log.CreatedBy));

            int appointmentHours = (log.Properties == null) ? 0 : (log.Properties.LaborHours ?? 0);
            decimal appointmentExpense = (log.Properties == null) ? 0 : (log.Properties.Expenses ?? 0);

            var appointmentToBeSaved = new Appointment()
            {
                AppointmentWorkloadWBID = workloadId,

                // guid generated
                AppointmentID = log.Id,
                AppointmentUserUniqueName = log.CreatedBy,
                AppointmentDate = log.CreatedDate,
                AppointmentComment = log.Text,

                // extra data
                AppointmentHoursDispensed = appointmentHours,
                AppointmentTE = appointmentExpense
            };

            _context.Appointments.Add(appointmentToBeSaved);

            var response = _context.SaveChanges();            
        }


        // advanced scenario

        public IEnumerable<WorkspaceItem> List(WorkspaceItemPropertiesFilter props)
        {
            var workloads = (from w in _context.WorkloadBacklogs
                             where (int)w.WBStatus < 3
                             select new WorkspaceItem
                             {
                                 Description = w.WBDescription,
                                 EndDate = w.WBEndDate,
                                 StartDate = w.WBStartDate,
                                 ItemState = (int)w.WBStatus,
                                 Id = w.WBID,
                                 Summary = "",
                                 Title = w.WBTitle,
                                 CreatedBy = w.WBCreatedBy,
                                 CreatedDate = w.WBCreatedDate,

                                 Properties = null
                             }).ToArray();

            return workloads.Select( w => LoadProperties(w, props) );
        }
        
        public WorkspaceItem TryGet(Guid itemId, WorkspaceItemPropertiesFilter props)
        {
            if (itemId == Guid.Empty)
                throw new ArgumentNullException(nameof(itemId));

            var workload = (from w in _context.WorkloadBacklogs
                            where w.WBID == itemId
                            select new WorkspaceItem
                            {
                                Description = w.WBDescription,
                                EndDate = w.WBEndDate,
                                StartDate = w.WBStartDate,
                                ItemState = (int)w.WBStatus,
                                Id = w.WBID,
                                Summary = "",
                                Title = w.WBTitle,
                                CreatedBy = w.WBCreatedBy,
                                CreatedDate = w.WBCreatedDate,

                                Properties = null
                            }).First();

            return LoadProperties(workload, props);
        }
        
        public WorkspaceItemProperties GetWorkloadItemProperties(Guid wbid, WorkspaceItemPropertiesFilter props)
        {
            var w = _context.WorkloadBacklogs.Find(wbid);

            var a = (from wb in _context.WorkloadBacklogs
                     select wb)
                    .ToArray();

            //var b = (from wb in a
            //         where wb.WBUsers. UserUniqueName != null
            //         select wb).ToArray();

            var a2 = a.ToArray();

            int i = 1;

            return new WorkspaceItemProperties()
            {
                ActivityID = props.HasActivityID ? (Guid?)w.WBActivityActivityID : null,
                StartDate = props.HasStartDate ? (DateTime?)w.WBStartDate : null,
                EndDate = props.HasEndDate ? (DateTime?)w.WBEndDate : null,
                Description = props.HasDescription ? w.WBDescription : null,
                Complexity = props.HasComplexity ? (int?)w.WBComplexity : null,
                Expertise = props.HasExpertise ? (int?)w.WBExpertise : null,

                IsWorkload = props.HasIsWorkload ? (bool?)w.WBIsWorkload : null,
                LastAppointmentId = props.HasLastAppointmentId ? w.LastAppointmentId : null,

                Files = (props.HasFiles && w.WBFiles != null) ? (from f in w.WBFiles
                                          select f.FileLink).ToArray() : null,

                Metrics = (props.HasMetrics && w.WBMetrics != null ) ? (from m in w.WBMetrics
                                              select m.MetricMetricID).ToArray() : null,

                Technologies = (props.HasTechnologies && w.WBTechnologies != null ) ? (from t in w.WBTechnologies
                                                        select t.TechnologyTechnologyId).ToArray() : null,

                WorkloadUsers = (props.HasWorkloadUsers && w.WBUsers != null) ? (from u in w.WBUsers
                                                                                 select u.UserUniqueName).ToArray() : null
            };
        }

        WorkspaceItem LoadProperties(WorkspaceItem workload, WorkspaceItemPropertiesFilter props)
        {
            Guid wbid = workload.Id;

            var w = _context.WorkloadBacklogs.First( ww => ww.WBID == wbid);
            
            workload.Properties = new WorkspaceItemProperties()
                {
                    ActivityID = props.HasActivityID ? (Guid?)w.WBActivityActivityID : null,
                    StartDate = props.HasStartDate ? (DateTime?)w.WBStartDate : null,
                    EndDate = props.HasEndDate ? (DateTime?)w.WBEndDate : null,
                    Description = props.HasDescription ? w.WBDescription : null,
                    Complexity = props.HasComplexity ? (int?)w.WBComplexity : null,
                    Expertise = props.HasExpertise ? (int?)w.WBExpertise : null,

                    IsWorkload = props.HasIsWorkload ? (bool?)w.WBIsWorkload : null,
                    LastAppointmentId = props.HasLastAppointmentId ? w.LastAppointmentId : null,

                    Files = (props.HasFiles && w.WBFiles != null) ? (from f in w.WBFiles
                                                                     select f.FileLink).ToArray() : null,

                    Metrics = (props.HasMetrics && w.WBMetrics != null) ? (from m in w.WBMetrics
                                                                           select m.MetricMetricID).ToArray() : null,

                    Technologies = (props.HasTechnologies && w.WBTechnologies != null) ? (from t in w.WBTechnologies
                                                                                          select t.TechnologyTechnologyId).ToArray() : null,

                    WorkloadUsers = (props.HasWorkloadUsers && w.WBUsers != null) ? (from u in w.WBUsers
                                                                                     select u.UserUniqueName).ToArray() : null
            };

            return workload;
        }

        IEnumerable<WorkspaceItem> LoadProperties(IEnumerable<WorkspaceItem> workload, WorkspaceItemPropertiesFilter props)
        {
            return workload.Select( w => LoadProperties(w, props) );
        }
    }
}
