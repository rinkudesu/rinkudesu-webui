using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace Rinkudesu.Gateways.Clients.Links;

[SuppressMessage("Performance", "CA1819:Properties should not return arrays")]
[ExcludeFromCodeCoverage]
public class LinkQueryDto
{
    public Guid[]? TagIds { get; set; }
    [SuppressMessage("Design", "CA1056:URI-like properties should not be strings", Justification = "Does not need to be a complete url to be considered correct")]
    public string? Url { get; set; }
    public string? Title { get; set; }
    public LinkListSortOptions? Sort { get; set; }
    public bool SortDescending { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }

    //todo: consider adding tests for this method once it becomes more complex than this or starts processing user-supplied bare strings
    internal string GenerateUriQueryString()
    {
        var queryArguments = new LinkedList<string>();
        // whenever any part of query string is generated using user-supplied data, make sure it's either safe to use (like GUID) or properly escaped before appending it to the query
        if (TagIds is { Length: > 0 })
        {
            queryArguments.AddLast(string.Join('&', TagIds.Select(t => $"tagIds={t.ToString()}")));
        }
        if (!string.IsNullOrWhiteSpace(Url))
        {
            queryArguments.AddLast($"urlContains={Uri.EscapeDataString(Url)}");
        }
        if (!string.IsNullOrWhiteSpace(Title))
        {
            queryArguments.AddLast($"titleContains={Uri.EscapeDataString(Title)}");
        }
        if (Sort.HasValue)
        {
            queryArguments.AddLast($"sortOptions={Uri.EscapeDataString(Sort.ToString()!)}");
        }
        if (SortDescending)
        {
            queryArguments.AddLast("sortDescending=true");
        }
        if (Skip.HasValue)
        {
            queryArguments.AddLast($"skip={Skip.Value.ToString(CultureInfo.InvariantCulture)}");
        }
        if (Take.HasValue)
        {
            queryArguments.AddLast($"take={Take.Value.ToString(CultureInfo.InvariantCulture)}");
        }
        queryArguments.AddLast("&showPrivate=true");
        return $"?{string.Join('&', queryArguments)}";
    }

    public enum LinkListSortOptions
    {
        [Display(Name = nameof(Resources.Links.LinkQueryDto.enumTitle), ResourceType = typeof(Resources.Links.LinkQueryDto))]
        Title,
        [Display(Name = nameof(Resources.Links.LinkQueryDto.enumUrl), ResourceType = typeof(Resources.Links.LinkQueryDto))]
        Url,
        [Display(Name = nameof(Resources.Links.LinkQueryDto.enumCreationDate), ResourceType = typeof(Resources.Links.LinkQueryDto))]
        CreationDate,
        [Display(Name = nameof(Resources.Links.LinkQueryDto.enumUpdateDate), ResourceType = typeof(Resources.Links.LinkQueryDto))]
        UpdateDate
    }
}
