using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Transactions;

namespace Api.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        RequestDelegate _next = next;
        ILogger<ExceptionMiddleware> _logger = logger;

        // IHostEnvironment env

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex?.Message ?? "Some thing went wrong");

                await HandleExceptionAsync(httpContext, ex!);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = exception switch
            {
                ArgumentNullException => (int)HttpStatusCode.BadRequest,              // 400 Bad Request
                ArgumentException => (int)HttpStatusCode.BadRequest,                 // 400 Bad Request
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,     // 401 Unauthorized
                KeyNotFoundException => (int)HttpStatusCode.NotFound,                // 404 Not Found
                FileNotFoundException => (int)HttpStatusCode.NotFound,               // 404 Not Found
                NotImplementedException => (int)HttpStatusCode.NotImplemented,       // 501 Not Implemented
                TimeoutException => (int)HttpStatusCode.RequestTimeout,              // 408 Request Timeout
                OperationCanceledException => (int)HttpStatusCode.RequestTimeout,    // 408 Request Timeout
                NullReferenceException => (int)HttpStatusCode.InternalServerError,   // 500 Internal Server Error
                InvalidOperationException => (int)HttpStatusCode.Conflict,           // 409 Conflict
                DivideByZeroException => (int)HttpStatusCode.InternalServerError,    // 500 Internal Server Error
                FormatException => (int)HttpStatusCode.BadRequest,                   // 400 Bad Request
                IndexOutOfRangeException => (int)HttpStatusCode.BadRequest,          // 400 Bad Request
                StackOverflowException => (int)HttpStatusCode.InternalServerError,   // 500 Internal Server Error
                OutOfMemoryException => (int)HttpStatusCode.InternalServerError,     // 500 Internal Server Error
                NotSupportedException => (int)HttpStatusCode.BadRequest,
                TransactionAbortedException => (int)HttpStatusCode.Conflict,         // 400 Bad Request
                _ => (int)HttpStatusCode.InternalServerError                         // Default: 500 Internal Server Error
            };
            var response = new
            {
                StatusCode = statusCode,
                Message = exception.Message,
                Detail = exception.InnerException?.Message
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var option = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, option);
            return context.Response.WriteAsync(json);
        }
    }

}