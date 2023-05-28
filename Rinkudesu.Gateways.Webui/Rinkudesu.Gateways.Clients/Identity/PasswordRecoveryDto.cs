using System;

namespace Rinkudesu.Gateways.Clients.Identity;

public class PasswordRecoveryDto
{
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
}
