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
    [Display(Name = "Tags")]
    //todo: this should be changed to an array as soon as multiple tags can be selected
    public Guid? TagIds { get; set; }
}
