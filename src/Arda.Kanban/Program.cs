using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Arda.Kanban
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
                .UseIISIntegration()
                .UseUrls("http://0.0.0.0:80", "http://0.0.0.0:81")
                .UseStartup<Startup>();

            string kestrelListen = config["KESTREL_LISTEN_KANBAN"];
            if ( kestrelListen != null )
            {
                System.Console.WriteLine("KESTREL_LISTEN_KANBAN = " + kestrelListen);

                builder = builder.UseUrls(kestrelListen);
            }

            var host = builder.Build();


            host.Run();
        }
    }
}
