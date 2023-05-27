using System.ComponentModel.DataAnnotations;

namespace Rinkudesu.Gateways.Webui.Models;

public class RegisterAccountViewModel
{
    [Required, DataType(DataType.EmailAddress)]
    [Display(Name = nameof(Resources.Models.Identity.RegisterAccountViewModel.email), ResourceType = typeof(Resources.Models.Identity.RegisterAccountViewModel))]
    public string Email { get; set; } = string.Empty;
    [Required, DataType(DataType.Password)]
    [Display(Name = nameof(Resources.Models.Identity.RegisterAccountViewModel.password), ResourceType = typeof(Resources.Models.Identity.RegisterAccountViewModel))]
    public string Password { get; set; } = string.Empty;
    [Required, DataType(DataType.Password)]
    [Display(Name = nameof(Resources.Models.Identity.RegisterAccountViewModel.passwordRepeat), ResourceType = typeof(Resources.Models.Identity.RegisterAccountViewModel))]
    public string PasswordRepeat { get; set; } = string.Empty;

    public bool PasswordMismatch => Password != PasswordRepeat;
}
