using Microsoft.AspNetCore.Http;
using System;

namespace Hera.Common.Core
{
    public class HeraExceptionResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public HeraExceptionResponse(Exception ex)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
            Message = ex.Message;
        }
    }
}
