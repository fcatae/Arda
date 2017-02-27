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

        public static TransactionalKanbanContext GetTransactionContext()
        {
            string connectionString = GetConnectionString();

            return TransactionalKanbanContext.Create(connectionString);
        }

        public static string SerializeObject(object obj)
        {
            if (obj == null)
                return "(null)";

            return JsonConvert.SerializeObject(obj, Formatting.Indented);
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

        public static void Validate<R>(ISupportSnapshot<R> testClass, 
                                            string testName, 
                                            Func<R[],KanbanContext,object> testFunction, 
                                            [System.Runtime.CompilerServices.CallerMemberName] string member = "")
        {
            using (var context = ArdaTestMgr.GetTransactionContext())
            {
                //FiscalYearRepository fiscalYear = new FiscalYearRepository(context);
                //var lista = fiscalYear.GetAllFiscalYears().ToArray();
                var before = testClass.GetSnapshot(context).ToArray();
                string beforeText = SerializeObject(before);

                //var fiscalYearId = lista.Min(r => r.FiscalYearID);
                //var row = fiscalYear.GetFiscalYearByID(fiscalYearId);

                var returnObject = testFunction(before, context);

                string returnObjectText = SerializeObject(returnObject);

                //var after = testClass.GetSnapshot(context);
                //string afterText = SerializeObject(after);

                string result = $"BEFORE:\n=======\n{beforeText}\n\nCOMMAND: {testName}\n\nAFTER:\n======\n{returnObjectText}";

                CompareResults(result, member);
                //ArdaTestMgr.CheckResult(row);
            }
        }

        public static void CompareResults(string result, string member)
        {
            string filename = $"files/{member}.json";

            try
            {
                string expected = ReadFile(filename);

                if (result != expected)
                {
                    throw new FailedTestException(member, null, result, expected);
                }
            }
            catch (IntegrationTestException)
            {
                if (AllowCreateResultFile)
                {
                    WriteFile(filename + ".result", result);
                }

                throw;
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
