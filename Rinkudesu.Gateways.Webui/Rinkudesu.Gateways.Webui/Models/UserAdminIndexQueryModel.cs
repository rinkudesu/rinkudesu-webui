using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace Rinkudesu.Gateways.Webui.Models;

[SuppressMessage("Performance", "CA1819:Properties should not return arrays")]
[BindProperties(SupportsGet = true)]
[ExcludeFromCodeCoverage]
public class UserAdminIndexQueryModel
{
    public SortOptions SortOption { get; set; }
    [Range(0, int.MaxValue)]
    public int Skip { get; set; }
    [Range(0, int.MaxValue)]
    public int Take { get; set; } = 20;
    public string? EmailContains { get; set; }
    public bool IsAdmin { get; set; }
    public bool? EmailConfirmed { get; set; }
    public bool LockedOnly { get; set; }

    public enum SortOptions
    {
        ByEmail,
    }
}
