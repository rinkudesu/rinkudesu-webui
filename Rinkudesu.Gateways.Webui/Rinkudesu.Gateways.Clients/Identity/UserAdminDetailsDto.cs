using System;

namespace Rinkudesu.Gateways.Clients.Identity;

public class UserAdminDetailsDto
{
    /// <summary>
    /// Id of the given user.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Email of the users. Doubles as a username.
    /// </summary>
    public string Email { get; set; } = null!;
    /// <summary>
    /// Indicates whether the user has confirmed their email.
    /// </summary>
    public bool EmailConfirmed { get; set; }
    /// <summary>
    /// Indicates whether the user has enabled two factor authentication.
    /// </summary>
    public bool TwoFactorEnabled { get; set; }
    /// <summary>
    /// Indicates whether the account has been locked out due to invalid login attempts.
    /// </summary>
    public bool AccountLockedOut { get; set; }
    /// <summary>
    /// Indicates whether the user has admin rights.
    /// </summary>
    public bool IsAdmin { get; set; }
}
