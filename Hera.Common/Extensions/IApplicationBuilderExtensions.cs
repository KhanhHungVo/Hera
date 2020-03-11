using Hera.Common.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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
    }
}
