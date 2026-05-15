using Microsoft.AspNetCore.Authorization;

namespace EdisonEngineering.API.Auth;

public class HasPermissionAttribute
    : AuthorizeAttribute
{
    public HasPermissionAttribute(
        string permission)
    {
        Policy = permission;
    }
}
