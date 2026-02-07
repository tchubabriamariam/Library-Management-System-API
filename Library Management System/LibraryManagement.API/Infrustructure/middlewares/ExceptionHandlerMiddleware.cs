
using Newtonsoft.Json;
using System.Net;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Formatting = Newtonsoft.Json.Formatting;

namespace LibraryManagement.API.Infrastructure.middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var error = new ApiError(context, ex);
            var result = JsonConvert.SerializeObject(error, (Formatting)Formatting.Indented);

            context.Response.Clear();
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = error.Status ?? (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsync(result);
        }
    }

    public class ApiError : ProblemDetails
    {
        public const string UnhandlerErrorCode = "UnhandledError";
        public string Code { get; set; }

        public string? TraceId
        {
            get => Extensions.TryGetValue("TraceId", out var traceId) ? traceId.ToString() : null;
            set => Extensions["TraceId"] = value;
        }

        public ApiError(HttpContext context, Exception exception)
        {
            TraceId = context.TraceIdentifier;
            Instance = context.Request.Path;
            Status = (int)HttpStatusCode.InternalServerError; // Default status 

            HandleException((dynamic)exception);
        }

        // Handle specific Library Exceptions
        private void HandleException(InvalidOperationException exception)
        {
            Code = "BusinessRuleViolation";
            Status = (int)HttpStatusCode.BadRequest;
            Title = exception.Message; // e.g., "Book is not available" 
        }


        private void HandleException(Exception exception)
        {
            Code = UnhandlerErrorCode;
            Status = (int)HttpStatusCode.InternalServerError;
            Title = "An unexpected error occurred on the server."; // Requirement 134 
        }
    }
}