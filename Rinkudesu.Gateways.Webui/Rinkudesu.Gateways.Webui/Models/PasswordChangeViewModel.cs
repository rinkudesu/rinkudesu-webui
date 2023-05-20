using System.ComponentModel.DataAnnotations;

namespace Rinkudesu.Gateways.Webui.Models;

public class PasswordChangeViewModel
{
    [Required, DataType(DataType.Password)]
    public string OldPassword { get; set; }
    [Required, DataType(DataType.Password)]
    public string NewPassword { get; set; }
    [Required, DataType(DataType.Password)]
    public string NewPasswordRepeat { get; set; }

    public bool NewPasswordsMatch => NewPassword == NewPasswordRepeat;
}
