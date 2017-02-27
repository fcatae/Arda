using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Arda.Kanban.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace IntegrationTests
{
    public class ArdaTestMgr
    {
        public static bool AllowCreateResultFile = false;
        private static IConfigurationRoot Configuration;

        static ArdaTestMgr()
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", true)
                .AddJsonFile("local-secret.json", true)
                .AddUserSecrets("IntegrationTests.Arda.Kanban.Repositories-20170226092351")
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        static string GetConnectionString()
        {
            return Configuration["Storage:SqlServer-Kanban:ConnectionString"];
        }

        public static KanbanContext GetTransactionContext()
        {
            string connectionString = GetConnectionString();

            return TransactionalKanbanContext.Create(connectionString);
        }

        public static string SerializeObject(object obj)
        {
            if (obj == null)
                return "(null)";

            return JsonConvert.SerializeObject(obj);
        }

        public static string ReadFile(string filename)
        {
            StreamReader reader = null;

            try
            {
                reader = File.OpenText(filename);
                return reader.ReadToEnd();
            }
            catch(FileNotFoundException)
            {
                throw new TestFileNotFoundException(filename);
            }
            finally
            {
                if(reader != null)
                    reader.Dispose();
            }
        }

        public static void WriteFile(string filename, string text)
        {
            using (var writer = File.CreateText(filename))
            {
                writer.Write(text);
            }
        }

        public static void CheckResult(object obj, [System.Runtime.CompilerServices.CallerMemberName] string member="")
        {
            string filename = $"files/{member}.json";

            string result = SerializeObject(obj);

            try
            {
                string expected = ReadFile(filename);

                if (result != expected)
                {
                    throw new FailedTestException(member, obj, result, expected);
                }
            }
            catch(IntegrationTestException)
            {
                if (AllowCreateResultFile)
                {
                    WriteFile(filename + ".result", result);
                }

                throw;
            }
        }
    }
}
