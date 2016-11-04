using Arda.Common.Models.Kanban;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arda.Kanban.Models
{
    public class KanbanContext : DbContext
    {
        public KanbanContext(DbContextOptions<KanbanContext> options)
            : base(options)
        { }

        public DbSet<FiscalYear> FiscalYears { get; set; }

        public DbSet<Metric> Metrics { get; set; }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<Technology> Technologies { get; set; }

        public DbSet<File> Files { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<WorkloadBacklog> WorkloadBacklogs { get; set; }

        public DbSet<WorkloadBacklogMetric> WorkloadBacklogMetrics { get; set; }

        public DbSet<WorkloadBacklogTechnology> WorkloadBacklogTechnologies { get; set; }

        public DbSet<WorkloadBacklogUser> WorkloadBacklogUsers { get; set; }

        public DbSet<Appointment> Appointments { get; set; }
    }
}