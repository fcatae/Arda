using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Arda.Reports
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", true)
                            .AddJsonFile("local-secret.json", true)
                            .AddJsonFile("microservices.json", true)
                            .AddEnvironmentVariables()
                            .AddCommandLine(args)
                            .Build();

            var builder = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUrls("http://0.0.0.0:8083")
                .UseIISIntegration()
                .UseStartup<Startup>();

            string kestrelListen = config["KESTREL_LISTEN_REPORTS"];
            if (kestrelListen != null)
            {
                System.Console.WriteLine("KESTREL_LISTEN_REPORTS = " + kestrelListen);

                builder = builder.UseUrls(kestrelListen);
            }

            var host = builder.Build();

            host.Run();
        }
    }
}