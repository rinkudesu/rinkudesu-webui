using System.ComponentModel.DataAnnotations;

namespace Rinkudesu.Gateways.Webui.Models;

public class PasswordChangeViewModel
{
    [Required, DataType(DataType.Password)]
    [Display(Name = nameof(Resources.Models.Identity.PasswordChangeViewModel.oldPassword), ResourceType = typeof(Resources.Models.Identity.PasswordChangeViewModel))]
    public string OldPassword { get; set; } = string.Empty;
    [Required, DataType(DataType.Password)]
    [Display(Name = nameof(Resources.Models.Identity.PasswordChangeViewModel.newPassword), ResourceType = typeof(Resources.Models.Identity.PasswordChangeViewModel))]
    public string NewPassword { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    [Display(Name = nameof(Resources.Models.Identity.PasswordChangeViewModel.newPasswordRepeat), ResourceType = typeof(Resources.Models.Identity.PasswordChangeViewModel))]
    public string NewPasswordRepeat { get; set; } = string.Empty;

    public bool NewPasswordsMatch => NewPassword == NewPasswordRepeat;
}
