using System.Diagnostics.CodeAnalysis;

namespace Rinkudesu.Gateways.Clients.Identity;

[ExcludeFromCodeCoverage]
public class AdminUserModificationDto
{
    public bool? Admin { get; set; }
    public bool? Locked { get; set; }
}
