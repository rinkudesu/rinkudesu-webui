﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Rinkudesu.Gateways.Clients.Tags;

public class TagDto
{
    public Guid Id { get; set; }

    [Display(Name = nameof(Resources.Tags.TagDto.name), ResourceType = typeof(Resources.Tags.TagDto))]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    [Required, MaxLength(7)]
    [Display(Name = nameof(Resources.Tags.TagDto.colour), ResourceType = typeof(Resources.Tags.TagDto))]
    public string Colour { get; set; } = "#edbd1e";
}
