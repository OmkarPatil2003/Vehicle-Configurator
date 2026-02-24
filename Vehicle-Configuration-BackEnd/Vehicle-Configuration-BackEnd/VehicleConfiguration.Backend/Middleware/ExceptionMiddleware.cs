using System.Net;
using System.Text.Json;
using VehicleConfiguration.Backend.DTOs;

namespace VehicleConfiguration.Backend.Middleware;

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
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new ErrorResponse
        {
            Timestamp = DateTime.UtcNow,
            Status = context.Response.StatusCode,
            Error = "Internal Server Error",
            Message = exception.Message, // In production, maybe hide this? User asked for same behavior as Spring Boot. Spring Boot default often shows message.
            Path = context.Request.Path
        };

        var json = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(json);
    }
}
