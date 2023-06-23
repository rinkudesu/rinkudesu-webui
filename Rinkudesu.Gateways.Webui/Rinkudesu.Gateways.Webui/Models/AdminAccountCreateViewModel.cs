using System.ComponentModel.DataAnnotations;

namespace Rinkudesu.Gateways.Webui.Models;

public class AdminAccountCreateViewModel
{
    [Required, DataType(DataType.EmailAddress)]
    [Display(Name = nameof(Resources.Models.Identity.AdminAccountCreateViewModel.email), ResourceType = typeof(Resources.Models.Identity.AdminAccountCreateViewModel))]
    public string Email { get; set; } = null!;

    [DataType(DataType.Password)]
    [Display(Name = nameof(Resources.Models.Identity.AdminAccountCreateViewModel.password), ResourceType = typeof(Resources.Models.Identity.AdminAccountCreateViewModel))]
    public string? Password { get; set; }
}
