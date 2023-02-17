using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace Rinkudesu.Gateways.Webui.Models;

[BindProperties(SupportsGet = true)]
[ExcludeFromCodeCoverage]
public class TagIndexQueryModel
{
    // the following properties don't need to be localised as they are never displayed to the user by name
    public int? Skip { get; set; } = 0;
    public int? Take { get; set; } = 20;
}
