using System.ComponentModel.DataAnnotations;

namespace Rinkudesu.Gateways.Webui.Models;

public class ChangeEmailViewModel
{
    [Required, DataType(DataType.EmailAddress)]
    [Display(Name = nameof(Resources.Models.Identity.ChangeEmailViewModel.email), ResourceType = typeof(Resources.Models.Identity.ChangeEmailViewModel))]
    public string Email { get; set; } = string.Empty;
}
