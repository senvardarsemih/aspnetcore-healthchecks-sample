using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace NetCoreHealthCheckSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseKestrel(options =>
                        {
                            options.AllowSynchronousIO = true;
                        })
                        .UseIISIntegration()
                        .UseIIS()
                        .UseStartup<Startup>();
                });
    }
}
