using Hera.Common.Core;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Hera.Common.Middleware
{
    public class HeraExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public HeraExceptionMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                string body = "Empty";
                if (context.Request.Body != null)
                {
                    using (StreamReader sr = new StreamReader(context.Request.Body))
                    {
                        body = await sr.ReadToEndAsync();
                    }
                }
                _logger.Error($"Error => {context.Request.Method} {context.Request.Path}{context.Request.QueryString}, Body => {body}");
                _logger.Error(ex, ex.Message);

                if (context.Response.HasStarted)
                {
                    _logger.Warning("The response has already started, the http status code middleware will not be executed.");
                    throw;
                }

                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new HeraExceptionResponse(ex)));
            }
        }
    }
}
