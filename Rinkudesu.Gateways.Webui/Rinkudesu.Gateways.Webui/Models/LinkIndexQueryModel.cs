using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Rinkudesu.Gateways.Clients.Links;

namespace Rinkudesu.Gateways.Webui.Models;

[SuppressMessage("Performance", "CA1819:Properties should not return arrays")]
[BindProperties(SupportsGet = true)]
[ExcludeFromCodeCoverage]
public class LinkIndexQueryModel
{
    [Display(Name = nameof(Resources.Models.Links.LinkIndexQueryModel.tags), ResourceType = typeof(Resources.Models.Links.LinkIndexQueryModel))]
    public Guid[]? TagIds { get; set; }
    [Display(Name = nameof(Resources.Models.Links.LinkIndexQueryModel.url), ResourceType = typeof(Resources.Models.Links.LinkIndexQueryModel))]
    public string? Url { get; set; }
    [Display(Name = nameof(Resources.Models.Links.LinkIndexQueryModel.title), ResourceType = typeof(Resources.Models.Links.LinkIndexQueryModel))]
    public string? Title { get; set; }
    [Display(Name = nameof(Resources.Models.Links.LinkIndexQueryModel.sort), ResourceType = typeof(Resources.Models.Links.LinkIndexQueryModel))]
    public LinkQueryDto.LinkListSortOptions? Sort { get; set; }
    [Display(Name = nameof(Resources.Models.Links.LinkIndexQueryModel.sortDescending), ResourceType = typeof(Resources.Models.Links.LinkIndexQueryModel))]
    public bool SortDescending { get; set; }
}
