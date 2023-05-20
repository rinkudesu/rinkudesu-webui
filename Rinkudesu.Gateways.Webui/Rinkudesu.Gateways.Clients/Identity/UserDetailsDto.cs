namespace Rinkudesu.Gateways.Clients.Identity;

public class UserDetailsDto
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool TwoFactorEnabled { get; set; }
}
