using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;


namespace TelephoneDirectory.Core.Extensions;
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // Ayrıntılı loglama burada yapılır
            LogException(context, ex);
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var errorDetails = new
        {
            Message = "An error occurred while processing your request.",
            DeveloperMessage = exception.Message,
            StackTrace = exception.StackTrace,
            RequestPath = context.Request.Path,
            RequestMethod = context.Request.Method
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(errorDetails));
    }

    private void LogException(HttpContext context, Exception exception)
    {
        _logger.LogError(exception,
            "An error occurred while processing the request. Path: {Path}, Method: {Method}, QueryString: {QueryString}",
            context.Request.Path,
            context.Request.Method,
            context.Request.QueryString);
    }
}




