using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Formatting.Compact;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Common.Extensions
{
    public static class WebHostBuilderContextExtensions
    {
        public static IWebHostBuilder UseHeraSerilog(this IWebHostBuilder webBuilder)
        {
            var loggerConfiguration = new LoggerConfiguration()
                                    .Enrich.WithProperty("Version", "1.0.0")
                                    .Enrich.WithThreadId()
                                    .WriteTo.File(new CompactJsonFormatter(), @"Logs\log.txt", Serilog.Events.LogEventLevel.Debug, rollingInterval: RollingInterval.Day);

            Log.Logger = loggerConfiguration.CreateLogger();
            webBuilder.ConfigureServices((context, services) =>
            {
                services.AddSingleton(Log.Logger);
            });
            return webBuilder;
        }
    }
}
