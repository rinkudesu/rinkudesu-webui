using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace Rinkudesu.Gateways.Webui.Models;

[SuppressMessage("Performance", "CA1819:Properties should not return arrays")]
[BindProperties(SupportsGet = true)]
[ExcludeFromCodeCoverage]
public class UserAdminIndexQueryViewModel
{
    [Display(Name = nameof(Resources.Models.Identity.UserAdminIndexQueryViewModel.sortOptions), ResourceType = typeof(Resources.Models.Identity.UserAdminIndexQueryViewModel))]
    public SortOptions SortOption { get; set; }
    [Range(0, int.MaxValue)]
    public int Skip { get; set; }
    [Range(0, int.MaxValue)]
    public int Take { get; set; } = 20;
    [Display(Name = nameof(Resources.Models.Identity.UserAdminIndexQueryViewModel.emailContains), ResourceType = typeof(Resources.Models.Identity.UserAdminIndexQueryViewModel))]
    public string? EmailContains { get; set; }
    [Display(Name = nameof(Resources.Models.Identity.UserAdminIndexQueryViewModel.isAdmin), ResourceType = typeof(Resources.Models.Identity.UserAdminIndexQueryViewModel))]
    public bool IsAdmin { get; set; }
    [Display(Name = nameof(Resources.Models.Identity.UserAdminIndexQueryViewModel.emailConfirmed), ResourceType = typeof(Resources.Models.Identity.UserAdminIndexQueryViewModel))]
    public bool? EmailConfirmed { get; set; }
    [Display(Name = nameof(Resources.Models.Identity.UserAdminIndexQueryViewModel.lockedOnly), ResourceType = typeof(Resources.Models.Identity.UserAdminIndexQueryViewModel))]
    public bool LockedOnly { get; set; }

    public enum SortOptions
    {
        ByEmail,
    }
}
