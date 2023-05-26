using System;
using System.Diagnostics.CodeAnalysis;

namespace Rinkudesu.Gateways.Clients.Identity;

[ExcludeFromCodeCoverage]
public class AccountCreatedDto
{
    public Guid UserId { get; set; }
    public string EmailConfirmationToken { get; set; }
}
