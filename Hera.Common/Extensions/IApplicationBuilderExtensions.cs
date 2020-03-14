using Hera.Common.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Hera.Common.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHeraExceptionMiddleware(this IApplicationBuilder appBuilder)
        {
            appBuilder.UseMiddleware<HeraExceptionMiddleware>();
            return appBuilder;
        }

        public static void UseHeraSwagger(this IApplicationBuilder appBuilder, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment()) return;
            
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            appBuilder.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            appBuilder.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "Hera API v1");
                s.DisplayRequestDuration();
            });
        }
    }
}
