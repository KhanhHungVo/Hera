using Hera.Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Hera.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseIISIntegration()
                              .UseStartup<Startup>()
                              .UseHeraSerilog();
                });
    }
}
