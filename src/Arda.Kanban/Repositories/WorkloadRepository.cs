using Arda.Kanban.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Arda.Kanban.Models.Repositories;
using Arda.Kanban.ViewModels;

namespace Arda.Kanban.Repositories
{
    public class WorkloadRepository : IWorkloadRepository
    {
        KanbanContext _context;

        public WorkloadRepository(KanbanContext context)
        {
            _context = context;
        }


        public bool AddNewWorkload(WorkloadViewModel workload)
        {
            //Load related Activity:
            var activity = _context.Activities.First(a => a.ActivityID == workload.WBActivity);
            //Load related Metrics:
            var metricList = new List<WorkloadBacklogMetric>();
            if (workload.WBMetrics != null)
                foreach (var mId in workload.WBMetrics)
                {
                    var metric = _context.Metrics.First(m => m.MetricID == mId);
                    metricList.Add(new WorkloadBacklogMetric()
                    {
                        Metric = metric
                    });
                }
            //Load related Technologies:
            var technologyList = new List<WorkloadBacklogTechnology>();
            if (workload.WBTechnologies != null)
                foreach (var tId in workload.WBTechnologies)
                {
                    var technology = _context.Technologies.First(t => t.TechnologyID == tId);
                    technologyList.Add(new WorkloadBacklogTechnology()
                    {
                        Technology = technology
                    });
                }
            //Load related Users:
            var userList = new List<WorkloadBacklogUser>();
            if (workload.WBUsers != null)
                foreach (var uniqueName in workload.WBUsers)
                {
                    var user = _context.Users.First(u => u.UniqueName == uniqueName);
                    userList.Add(new WorkloadBacklogUser()
                    {
                        User = user
                    });
                }
            //Associate related Files:
            var filesList = new List<File>();
            if (workload.WBFilesList != null)
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
            //Create workload object:
            var workloadToBeSaved = new WorkloadBacklog()
            {
                WBActivity = activity,
                WBAppointments = null,
                WBComplexity = (Complexity)workload.WBComplexity,
                WBCreatedBy = workload.WBCreatedBy,
                WBCreatedDate = workload.WBCreatedDate,
                WBDescription = workload.WBDescription,
                WBEndDate = workload.WBEndDate,
                WBExpertise = (Expertise)workload.WBExpertise,
                WBFiles = filesList,
                WBID = workload.WBID,
                WBIsWorkload = workload.WBIsWorkload,
                WBMetrics = metricList,
                WBStartDate = workload.WBStartDate,
                WBStatus = (Status)workload.WBStatus,
                WBTechnologies = technologyList,
                WBTitle = workload.WBTitle,
                WBUsers = userList
            };

            _context.WorkloadBacklogs.Add(workloadToBeSaved);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteWorkloadByID(Guid id)
        {
            //Get files:
            var files = _context.Files.Where(f => f.WorkloadBacklog.WBID == id);
            //Load Metrics:
            var metrics = _context.WorkloadBacklogMetrics.Where(wbm => wbm.WorkloadBacklog.WBID == id);
            //Load Technologies:
            var technologies = _context.WorkloadBacklogTechnologies.Where(wbt => wbt.WorkloadBacklog.WBID == id);
            //Load Users:
            var users = _context.WorkloadBacklogUsers.Where(wbu => wbu.WorkloadBacklog.WBID == id);
            //Get Workload:
            var workload = _context.WorkloadBacklogs.First(w => w.WBID == id);
            //Remove Everything:
            _context.Files.RemoveRange(files);
            _context.WorkloadBacklogMetrics.RemoveRange(metrics);
            _context.WorkloadBacklogTechnologies.RemoveRange(technologies);
            _context.WorkloadBacklogUsers.RemoveRange(users);
            _context.WorkloadBacklogs.Remove(workload);
            _context.SaveChanges();
            return true;
        }

        public bool UpdateWorkloadStatus(Guid id, int status)
        {
            var workload = _context.WorkloadBacklogs.First(w => w.WBID == id);
            workload.WBStatus = (Status)status;
            _context.SaveChanges();

            return true;
        }

        public bool EditWorkload(WorkloadViewModel workload)
        {
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

            if( workload.WBTechnologies != null )
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

        public IEnumerable<WorkloadViewModel> GetAllWorkloads()
        {
            var workloads = (from w in _context.WorkloadBacklogs
                             join a in _context.Activities on w.WBActivity.ActivityID equals a.ActivityID
                             select new WorkloadViewModel()
                             {
                                 WBActivity = a.ActivityID,
                                 WBComplexity = (int)w.WBComplexity,
                                 WBCreatedBy = w.WBCreatedBy,
                                 WBCreatedDate = w.WBCreatedDate,
                                 WBDescription = w.WBDescription,
                                 WBEndDate = w.WBEndDate,
                                 WBExpertise = (int)w.WBExpertise,
                                 WBFilesList = (from f in _context.Files
                                                where f.WorkloadBacklog.WBID == w.WBID
                                                orderby f.FileName
                                                select new Tuple<Guid, string, string>(f.FileID, f.FileLink, f.FileName)
                                               ),
                                 WBID = w.WBID,
                                 WBIsWorkload = w.WBIsWorkload,
                                 WBMetrics = (from wbm in _context.WorkloadBacklogMetrics
                                              join m in _context.Metrics on wbm.Metric equals m
                                              where wbm.WorkloadBacklog.WBID == w.WBID
                                              orderby wbm.Metric.MetricCategory, wbm.Metric.MetricName
                                              select new Guid(wbm.Metric.MetricID.ToString())),
                                 WBStartDate = w.WBStartDate,
                                 WBStatus = (int)w.WBStatus,
                                 WBTechnologies = (from wbt in _context.WorkloadBacklogTechnologies
                                                   join t in _context.Technologies on wbt.Technology equals t
                                                   where wbt.WorkloadBacklog.WBID == w.WBID
                                                   orderby t.TechnologyName
                                                   select new Guid(wbt.Technology.TechnologyID.ToString())),
                                 WBTitle = w.WBTitle,
                                 WBUsers = (from wbu in _context.WorkloadBacklogUsers
                                            join u in _context.Users on wbu.User equals u
                                            where wbu.WorkloadBacklog.WBID == w.WBID
                                            orderby u.UniqueName
                                            select wbu.User.UniqueName)
                             });
            return workloads;
        }

        public WorkloadViewModel GetWorkloadByID(Guid id)
        {
            if (id != null)
            {
                var workload = (from w in _context.WorkloadBacklogs
                                join a in _context.Activities on w.WBActivity.ActivityID equals a.ActivityID
                                where w.WBID == id
                                select new WorkloadViewModel()
                                {
                                    WBActivity = a.ActivityID,
                                    WBComplexity = (int)w.WBComplexity,
                                    WBCreatedBy = w.WBCreatedBy,
                                    WBCreatedDate = w.WBCreatedDate,
                                    WBDescription = w.WBDescription,
                                    WBEndDate = w.WBEndDate,
                                    WBExpertise = (int)w.WBExpertise,
                                    WBFilesList = (from f in _context.Files
                                                   where f.WorkloadBacklog.WBID == w.WBID
                                                   orderby f.FileName
                                                   select new Tuple<Guid, string, string>(f.FileID, f.FileLink, f.FileName)
                                                  ),
                                    WBID = w.WBID,
                                    WBIsWorkload = w.WBIsWorkload,
                                    WBMetrics = (from wbm in _context.WorkloadBacklogMetrics
                                                 join m in _context.Metrics on wbm.Metric equals m
                                                 where wbm.WorkloadBacklog.WBID == w.WBID
                                                 orderby wbm.Metric.MetricCategory, wbm.Metric.MetricName
                                                 select new Guid(wbm.Metric.MetricID.ToString())),
                                    WBStartDate = w.WBStartDate,
                                    WBStatus = (int)w.WBStatus,
                                    WBTechnologies = (from wbt in _context.WorkloadBacklogTechnologies
                                                      join t in _context.Technologies on wbt.Technology equals t
                                                      where wbt.WorkloadBacklog.WBID == w.WBID
                                                      orderby t.TechnologyName
                                                      select new Guid(wbt.Technology.TechnologyID.ToString())),
                                    WBTitle = w.WBTitle,
                                    WBUsers = (from wbu in _context.WorkloadBacklogUsers
                                               join u in _context.Users on wbu.User equals u
                                               where wbu.WorkloadBacklog.WBID == w.WBID
                                               orderby u.UniqueName
                                               select wbu.User.UniqueName)
                                }).First();

                return workload;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<WorkloadsByUserViewModel> GetWorkloadsByUser(string uniqueName)
        {
            var workloads = (from wb in _context.WorkloadBacklogs
                             join wbu in _context.WorkloadBacklogUsers on wb.WBID equals wbu.WorkloadBacklogWBID
                             // wb.WBUsers.Where(u => u.User.UniqueName == uniqueName).First().WBUserID equals wbu.WBUserID
                             join uk in _context.Users on wbu.User.UniqueName equals uk.UniqueName
                             //join at in _context.Files on wb equals at.WorkloadBacklog
                             where uk.UniqueName.Equals(uniqueName)
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
                                                   select new Tuple<string, string>(wbusers.User.UniqueName, wbusers.User.Name)).ToList()
                             }).ToList();

            foreach (var w in workloads)
            {
                var hours = (from a in _context.Appointments
                             where a.AppointmentWorkload.WBID == w._WorkloadID
                             select a.AppointmentHoursDispensed).ToList();
                w._WorkloadHours = hours.Sum();
            }

            if (workloads != null)
            {
                return workloads;
            }
            else
            {
                return null;
            }
        }

        [Obsolete]
        public bool SendNotificationAboutNewOrUpdatedWorkload(string uniqueName, int newOrUpdate)
        {
            return true;
            //EmailLogic clientEmail = new EmailLogic();

            //// Mounting parameters and message.
            //string ToName = Util.GetUserAlias(uniqueName);
            //string ToEmail = uniqueName;
            //string Subject = "";

            //StringBuilder StructureModified = new StringBuilder();
            //StructureModified = EmailMessages.GetEmailMessageStructure();

            //// Replacing the generic title by the customized.
            //StructureModified.Replace("[MessageTitle]", "Hi " + ToName + ", we have news to you: ");

            //if (newOrUpdate == 0) // new workload
            //{
            //    Subject += "[ARDA] New workload available to you";

            //    // Replacing the generic subtitle by the customized.
            //    StructureModified.Replace("[MessageSubtitle]", "A new <strong>Workload</strong> was created and signed to you.");

            //    // Replacing the generic message body by the customized.
            //    StructureModified.Replace("[MessageBody]", "To see your new workload, please, click on the link bellow.");
            //}
            //else
            //{
            //    Subject += "[ARDA] Existing workload has been updated";

            //    // Replacing the generic subtitle by the customized.
            //    StructureModified.Replace("[MessageSubtitle]", "A existent <strong>Workload</strong> signed to you was updated.");

            //    // Replacing the generic message body by the customized.
            //    StructureModified.Replace("[MessageBody]", "To see your workload updated, please, click on the link bellow.");
            //}

            //// Replacing the generic callout box.
            //StructureModified.Replace("[MessageCallout]", "Dashboard (Kanban): <a href='/Dashboard/Index'>My workloads</a>");

            //// Creating a object that will send the message.
            //EmailLogic EmailObject = new EmailLogic();

            //try
            //{
            //    var EmailTask = EmailObject.SendEmailAsync(ToName, ToEmail, Subject, StructureModified.ToString());
            //    return true;
            //}
            //catch (Exception)
            //{
            //    return false;
            //}
        }
    }
}
