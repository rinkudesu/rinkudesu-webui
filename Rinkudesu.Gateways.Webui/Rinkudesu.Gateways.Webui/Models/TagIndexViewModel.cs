using System;

namespace Rinkudesu.Gateways.Webui.Models;

public class TagIndexViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Colour { get; set; } = "#edbd1e";
}
