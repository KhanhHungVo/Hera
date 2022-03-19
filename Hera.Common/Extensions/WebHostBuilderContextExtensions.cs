using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Formatting.Compact;

namespace Hera.Common.Extensions
{
    public static class WebHostBuilderContextExtensions
    {
        public static IWebHostBuilder UseHeraSerilog(this IWebHostBuilder webBuilder)
        {

            Log.Logger = new LoggerConfiguration()
                                    .Enrich.WithProperty("Version", "1.0.0")
                                    .Enrich.WithThreadId()
                                    .WriteTo.File(new CompactJsonFormatter(), @"Logs\log.txt", Serilog.Events.LogEventLevel.Debug, rollingInterval: RollingInterval.Day)
                                    .CreateLogger();
            webBuilder.ConfigureServices((context, services) =>
            {
                services.AddSingleton(Log.Logger);
            });
            
            
            return webBuilder;
        }
    }
}
