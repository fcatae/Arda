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
        public class TestFileNotFoundException : Exception
        {
            public TestFileNotFoundException(string message) : base(message)
            {
            }
        }

        public class FailedTestException : Exception
        {
            private string _expectedText;
            private string _message;
            private object _resultObject;
            private string _resultText;

            public FailedTestException(string message, object resultObject, string resultText, string expectedText) : base(message)
            {
                this._message = message;
                this._resultObject = resultObject;
                this._resultText = resultText;
                this._expectedText = expectedText;
            }
        }

        public static bool AllowCreateResultFile = false;
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

        public static string SerializeObject(object obj)
        {
            if (obj == null)
                return "(null)";

            return JsonConvert.SerializeObject(obj);
        }

        public static string ReadFile(string filename)
        {
            using (var reader = File.OpenText(filename))
            {
                return reader.ReadToEnd();
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
                    throw new FailedTestException(member, obj, result, expected);
            }
            catch(FileNotFoundException) when (AllowCreateResultFile)
            {
                WriteFile(filename + ".result", result);

                throw new TestFileNotFoundException(filename);
            }
        }
    }
}
