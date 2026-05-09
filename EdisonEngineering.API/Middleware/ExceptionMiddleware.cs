using System.Net;
using System.Text.Json;
using EdisonEngineering.Application.Common;

namespace EdisonEngineering.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger,
        IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var traceId = context.TraceIdentifier;

            _logger.LogError(
                ex,
                "Unhandled exception occurred. TraceId: {TraceId}, Path: {Path}, Method: {Method}",
                traceId,
                context.Request.Path,
                context.Request.Method);

            context.Response.StatusCode =
                (int)HttpStatusCode.InternalServerError;

            context.Response.ContentType =
                "application/json";

            var response = new ApiResponse<object>
            {
                Success = false,
                Message = "Internal server error",
                Errors = new List<string>()
            };

            // Show actual error only in Development
            if (_environment.IsDevelopment())
            {
                response.Errors.Add(ex.Message);

                if (ex.InnerException != null)
                {
                    response.Errors.Add(ex.InnerException.Message);
                }
            }

            var json = JsonSerializer.Serialize(
                response,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            await context.Response.WriteAsync(json);
        }
    }
}