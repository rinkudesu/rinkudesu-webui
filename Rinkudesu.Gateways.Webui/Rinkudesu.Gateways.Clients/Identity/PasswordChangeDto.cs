using System.ComponentModel.DataAnnotations;

namespace Rinkudesu.Gateways.Clients.Identity;

public class PasswordChangeDto
{
    [Required]
    public string OldPassword { get; set; }
    [Required]
    public string NewPassword { get; set; }
    [Required]
    public string NewPasswordRepeat { get; set; }

    public bool NewPasswordsMatch => NewPassword == NewPasswordRepeat;
}
