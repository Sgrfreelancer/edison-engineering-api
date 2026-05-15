using System.Security.Claims;

using EdisonEngineering.Infrastructure.Persistence;

using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace EdisonEngineering.API.Auth;

public class PermissionHandler
    : AuthorizationHandler<PermissionRequirement>
{
    private readonly AppDbContext _context;

    public PermissionHandler(
        AppDbContext context)
    {
        _context = context;
    }

    protected override async Task
        HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
    {
        var role =
            context.User.FindFirst(
                ClaimTypes.Role)?.Value;

        if (string.IsNullOrWhiteSpace(role))
        {
            return;
        }

        var hasPermission =
            await _context.RolePermissions
                .Include(x => x.Permission)
                .AnyAsync(x =>
                    x.Role == role &&
                    x.Permission.Code
                        == requirement.Permission);

        if (hasPermission)
        {
            context.Succeed(requirement);
        }
    }
}
