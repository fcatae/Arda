using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Arda.Reports
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseUrls("http://0.0.0.0:80","http://0.0.0.0:83")
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}