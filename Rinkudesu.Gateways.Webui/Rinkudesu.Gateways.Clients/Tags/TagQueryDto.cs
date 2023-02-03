using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Rinkudesu.Gateways.Clients.Tags;

[ExcludeFromCodeCoverage]
[SuppressMessage("Performance", "CA1819:Properties should not return arrays")]
public class TagQueryDto
{
    public string? Name { get; set; }

    public static readonly TagQueryDto Empty = new();

    internal string GenerateUriQueryString()
    {
        var queryArguments = new LinkedList<string>();

        if (!string.IsNullOrWhiteSpace(Name))
        {
            queryArguments.AddLast($"name={Uri.EscapeDataString(Name)}");
        }

        return $"?{string.Join('&', queryArguments)}";
    }
}
