using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Arda.Permissions
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseUrls("http://0.0.0.0:80","http://0.0.0.0:82")                
                .UseStartup<Startup>()
                .Build();

            host.Run(); //running
        }
    }
}
