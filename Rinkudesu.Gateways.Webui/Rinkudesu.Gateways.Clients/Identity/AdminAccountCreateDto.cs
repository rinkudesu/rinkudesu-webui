namespace Rinkudesu.Gateways.Clients.Identity;
/// <summary>
/// A DTO class used for manually creating a user account by an administrator.
/// </summary>
public class AdminAccountCreateDto
{
    /// <summary>
    /// Email address of the new account.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Password to set for the new user. If empty, then a password recovery will need to be used to log in for the first time.
    /// </summary>
    public string? Password { get; set; }
}
