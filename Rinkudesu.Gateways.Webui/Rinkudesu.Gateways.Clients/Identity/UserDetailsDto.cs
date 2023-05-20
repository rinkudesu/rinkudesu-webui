namespace Rinkudesu.Gateways.Clients.Identity;

public class UserDetailsDto
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public bool TwoFactorEnabled { get; set; }
}
