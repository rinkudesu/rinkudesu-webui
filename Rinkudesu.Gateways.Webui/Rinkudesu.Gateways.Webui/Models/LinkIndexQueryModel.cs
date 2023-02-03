using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace Rinkudesu.Gateways.Webui.Models;

[SuppressMessage("Performance", "CA1819:Properties should not return arrays")]
[BindProperties(SupportsGet = true)]
[ExcludeFromCodeCoverage]
public class LinkIndexQueryModel
{
    [Display(Name = nameof(Resources.Models.Links.LinkIndexQueryModel.tags), ResourceType = typeof(Resources.Models.Links.LinkIndexQueryModel))]
    public Guid[]? TagIds { get; set; }
}
