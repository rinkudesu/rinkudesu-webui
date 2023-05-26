using System.ComponentModel.DataAnnotations;

namespace Rinkudesu.Gateways.Clients.Identity;

public class RegisterAccountDto
{
    [Required, DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string PasswordRepeat { get; set; }

    public bool PasswordMismatch => Password != PasswordRepeat;
}
