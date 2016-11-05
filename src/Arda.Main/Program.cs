using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Arda.Main
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel(
                options =>
                {
                    options.NoDelay = true;
                    options.UseHttps("arda.pfx", "Pa$$w0rd");
                    options.UseConnectionLogging();
                }
                )
                .UseUrls("http://0.0.0.0:80", "https://0.0.0.0:443")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run(); //running
        }
    }
}
