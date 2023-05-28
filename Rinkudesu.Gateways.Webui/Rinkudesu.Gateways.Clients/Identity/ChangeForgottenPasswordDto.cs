using System;

namespace Rinkudesu.Gateways.Clients.Identity;

public class ChangeForgottenPasswordDto
{
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string PasswordRepeat { get; set; } = string.Empty;
}
