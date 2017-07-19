using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Arda.Main
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
                .UseKestrel(options =>
                {
                    options.NoDelay = true;
                    options.UseHttps("arda.pfx", "Pa$$w0rd");
                    options.UseConnectionLogging();
                })

                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUrls("http://*:1265","https://*:44304")
                .UseIISIntegration()
                .UseStartup<Startup>();

            string kestrelListen = config["KESTREL_LISTEN_MAIN"];
            if (kestrelListen != null)
            {
                System.Console.WriteLine("KESTREL_LISTEN_MAIN = " + kestrelListen);

                builder = builder.UseUrls(kestrelListen);
            }

            var host = builder.Build();

            host.Run();            
        }
    }
}
