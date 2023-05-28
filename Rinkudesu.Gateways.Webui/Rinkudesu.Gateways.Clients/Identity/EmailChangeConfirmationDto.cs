using System;

namespace Rinkudesu.Gateways.Clients.Identity;

public class EmailChangeConfirmationDto
{
    public Guid UserId { get; set; }
    public string NewEmail { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
