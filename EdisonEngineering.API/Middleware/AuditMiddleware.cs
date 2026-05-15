using System.Security.Claims;

using EdisonEngineering.Domain.Entities;
using EdisonEngineering.Infrastructure.Persistence;

namespace EdisonEngineering.API.Middleware;

public class AuditMiddleware
{
    private readonly RequestDelegate _next;

    public AuditMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        AppDbContext dbContext)
    {
        await _next(context);

        // ❌ Skip Swagger
        if (context.Request.Path
            .StartsWithSegments("/swagger"))
        {
            return;
        }

        // ❌ Skip static files
        if (context.Request.Path
            .StartsWithSegments("/favicon"))
        {
            return;
        }

        // ❌ Skip health checks
        if (context.Request.Path
            .StartsWithSegments("/health"))
        {
            return;
        }

        var userId =
            context.User.FindFirst(
                ClaimTypes.NameIdentifier)
                    ?.Value;

        var email =
            context.User.FindFirst(
                ClaimTypes.Email)
                    ?.Value;

        var audit = new AuditLog
        {
            UserId = userId,

            UserEmail = email,

            Method = context.Request.Method,

            Path = context.Request.Path,

            IpAddress =
                context.Connection
                    .RemoteIpAddress
                    ?.ToString(),

            StatusCode =
                context.Response.StatusCode
        };

        dbContext.AuditLogs.Add(audit);

        await dbContext.SaveChangesAsync();
    }
}
