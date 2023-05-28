using System;
using System.ComponentModel.DataAnnotations;

namespace Rinkudesu.Gateways.Webui.Models;

public class ChangeForgottenPasswordViewModel
{
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public string Token { get; set; } = string.Empty;
    [Required, DataType(DataType.Password)]
    [Display(Name = nameof(Resources.Models.Identity.ChangeForgottenPassword.password), ResourceType = typeof(Resources.Models.Identity.ChangeForgottenPassword))]
    public string Password { get; set; } = string.Empty;
    [Required, DataType(DataType.Password)]
    [Display(Name = nameof(Resources.Models.Identity.ChangeForgottenPassword.passwordRepeat), ResourceType = typeof(Resources.Models.Identity.ChangeForgottenPassword))]
    public string PasswordRepeat { get; set; } = string.Empty;

    public bool PasswordMismatch => Password != PasswordRepeat;
}
