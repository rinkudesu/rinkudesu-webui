using System.ComponentModel.DataAnnotations;

namespace Rinkudesu.Gateways.Webui.Models;

public class ForgotPasswordViewModel
{
    [Required, DataType(DataType.EmailAddress)]
    [Display(Name = nameof(Resources.Models.Identity.ForgotPasswordViewModel.email), ResourceType = typeof(Resources.Models.Identity.ForgotPasswordViewModel))]
    public string Email { get; set; } = string.Empty;
}
