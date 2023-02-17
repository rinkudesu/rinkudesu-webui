using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Rinkudesu.Gateways.Clients.Tags;

[ExcludeFromCodeCoverage]
[SuppressMessage("Performance", "CA1819:Properties should not return arrays")]
public class TagQueryDto
{
    [JsonPropertyName("Name")]
    public string? NameQuery { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }

    public static readonly TagQueryDto Empty = new();

    internal string GenerateUriQueryString()
    {
        var queryArguments = new LinkedList<string>();

        if (!string.IsNullOrWhiteSpace(NameQuery))
        {
            queryArguments.AddLast($"name={Uri.EscapeDataString(NameQuery)}");
        }
        if (Skip.HasValue)
        {
            queryArguments.AddLast($"offset={Skip.Value.ToString(CultureInfo.InvariantCulture)}");
        }
        if (Take.HasValue)
        {
            queryArguments.AddLast($"limit={Take.Value.ToString(CultureInfo.InvariantCulture)}");
        }

        return $"?{string.Join('&', queryArguments)}";
    }
}
