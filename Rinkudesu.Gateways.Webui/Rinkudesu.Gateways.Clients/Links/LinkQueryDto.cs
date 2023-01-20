using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Rinkudesu.Gateways.Clients.Links;

[SuppressMessage("Performance", "CA1819:Properties should not return arrays")]
[ExcludeFromCodeCoverage]
public class LinkQueryDto
{
    public Guid[]? TagIds { get; set; }

    //todo: consider adding tests for this method once it becomes more complex than this or starts processing user-supplied bare strings
    internal string GenerateUriQueryString()
    {
        var queryArguments = new LinkedList<string>();
        // whenever any part of query string is generated using user-supplied data, make sure it's either safe to use (like GUID) or properly escaped before appending it to the query
        if (TagIds is { Length: > 0 })
        {
            queryArguments.AddLast(string.Join('&', TagIds.Select(t => $"tagIds={t.ToString()}")));
        }
        queryArguments.AddLast("&showPrivate=true");
        return $"?{string.Join('&', queryArguments)}";
    }
}
