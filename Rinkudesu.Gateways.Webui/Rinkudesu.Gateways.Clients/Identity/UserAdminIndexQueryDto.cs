using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Rinkudesu.Gateways.Utils;

namespace Rinkudesu.Gateways.Clients.Identity;

[SuppressMessage("Performance", "CA1819:Properties should not return arrays")]
[ExcludeFromCodeCoverage]
public class UserAdminIndexQueryDto
{
    public SortOptions SortOption { get; set; }
    [Range(0, int.MaxValue)]
    public int? Skip { get; set; }
    [Range(0, int.MaxValue)]
    public int? Take { get; set; }
    public string? EmailContains { get; set; }
    public bool IsAdmin { get; set; }
    public bool? EmailConfirmed { get; set; }
    public bool LockedOnly { get; set; }

    public enum SortOptions
    {
        ByEmail,
    }

    internal string GetQueryString()
    {
        var queryItems = new LinkedList<string>();
        // using hard-coded parameter names instead of nameof to prevent accidental changes by changing the property names here
        queryItems.AddLast($"SortOption={Enum.GetName(SortOption)}");
        queryItems.AddLast($"Skip={(Skip ?? 0).ToString(CultureInfo.InvariantCulture)}");
        queryItems.AddLast($"Take={(Take ?? 20).ToString(CultureInfo.InvariantCulture)}");
        if (!string.IsNullOrEmpty(EmailContains))
            queryItems.AddLast($"EmailContains={EmailContains.Trim().UriQueryEncode()}");
        queryItems.AddLast($"IsAdmin={IsAdmin.ToString()}");
        if (EmailConfirmed.HasValue)
            queryItems.AddLast($"EmailConfirmed={EmailConfirmed.Value.ToString()}");
        queryItems.AddLast($"LockedOnly={LockedOnly.ToString()}");

        return $"?{string.Join('&', queryItems)}";
    }
}
