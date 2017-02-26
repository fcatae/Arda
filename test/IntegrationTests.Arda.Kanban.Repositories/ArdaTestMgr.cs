using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Arda.Kanban.Models;
using Microsoft.Extensions.Configuration;

namespace IntegrationTests
{
    public class ArdaTestMgr
    {
        private static IConfigurationRoot Configuration;

        static ArdaTestMgr()
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", true)
                .AddJsonFile("local-secret.json", true)
                .AddJsonFile("microservices.json", true)
                .AddUserSecrets("arda-20160816073715")
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        static string GetConnectionString()
        {
            return Configuration["Storage:SqlServer-Kanban:ConnectionString"];
        }

        public static KanbanContext GetDbContext()
        {
            string connectionString = GetConnectionString();

            var opts = (new DbContextOptionsBuilder<KanbanContext>())
                            .UseSqlServer(connectionString);

            return new KanbanContext(opts.Options);
        }
    }
}
