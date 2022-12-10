using System;
using System.ComponentModel.DataAnnotations;

namespace Rinkudesu.Gateways.Clients.LinkTags;

public class LinkTagDto
{
    [Required]
    public Guid LinkId { get; set; }
    [Required]
    public Guid? TagId { get; set; }
}
