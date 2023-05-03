using System.ComponentModel.DataAnnotations;

namespace Rinkudesu.Gateways.Webui.Models;

public class UserLoginViewModel
{
    [Required]
    [Display(Name = nameof(Resources.Models.UserSession.UserLoginViewModel.username), ResourceType = typeof(Resources.Models.UserSession.UserLoginViewModel))]
    public string UserName { get; set; } = string.Empty;

    [DataType(DataType.Password), Required]
    [Display(Name = nameof(Resources.Models.UserSession.UserLoginViewModel.password), ResourceType = typeof(Resources.Models.UserSession.UserLoginViewModel))]
    public string Password { get; set; } = string.Empty;
}
