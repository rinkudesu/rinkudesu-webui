using System;

namespace Rinkudesu.Gateways.Clients.Identity;

public class ConfirmEmailChangeDto
{
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public string NewEmail { get; set; } = string.Empty;
}
