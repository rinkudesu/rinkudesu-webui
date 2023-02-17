using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace Rinkudesu.Gateways.Webui.Models;

[BindProperties(SupportsGet = true)]
[ExcludeFromCodeCoverage]
public class TagIndexQueryModel
{
    [Display(Name = nameof(Resources.Models.Tags.TagIndexQueryModel.name), ResourceType = typeof(Resources.Models.Tags.TagIndexQueryModel))]
    public string? NameQuery { get; set; }
    // the following properties don't need to be localised as they are never displayed to the user by name
    public int? Skip { get; set; } = 0;
    public int? Take { get; set; } = 20;
}
