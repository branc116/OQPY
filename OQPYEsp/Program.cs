using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace OQPYEsp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Task t = Task.Factory.StartNew(async () =>
            {
                await Services.HandleQ.PushQ();
            }); 
            var host = new WebHostBuilder()
                .UseUrls("http://0.0.0.0:5000")
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
            host.Run();
        }
    }
}