using System.ComponentModel.DataAnnotations;

namespace Rinkudesu.Gateways.Clients.Identity;

public class RegisterAccountDto
{
    [Required, DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    [Required]
    public string PasswordRepeat { get; set; } = string.Empty;

    public bool PasswordMismatch => Password != PasswordRepeat;
}
