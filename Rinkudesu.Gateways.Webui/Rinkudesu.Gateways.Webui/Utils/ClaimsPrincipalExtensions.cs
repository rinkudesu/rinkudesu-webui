using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Rinkudesu.Gateways.Webui.Utils;

[ExcludeFromCodeCoverage]
public static class ClaimsPrincipalExtensions
{
    public static bool IsAdmin(this ClaimsPrincipal user)
        => user.IsInRole(Roles.Admin);
}
