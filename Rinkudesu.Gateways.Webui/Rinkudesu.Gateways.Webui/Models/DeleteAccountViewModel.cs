using System.ComponentModel.DataAnnotations;

namespace Rinkudesu.Gateways.Webui.Models;

public class DeleteAccountViewModel
{
    [Required, DataType(DataType.Password)]
    [Display(Name = nameof(Resources.Models.Identity.DeleteAccountViewModel.password), ResourceType = typeof(Resources.Models.Identity.DeleteAccountViewModel))]
    public string Password { get; set; } = string.Empty;
}
