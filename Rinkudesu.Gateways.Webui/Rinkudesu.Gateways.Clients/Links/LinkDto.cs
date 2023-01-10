using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Rinkudesu.Gateways.Clients.Tags;

namespace Rinkudesu.Gateways.Clients.Links
{
    public sealed class LinkDto
    {
        public Guid Id { get; set; }
        [Display(Name = nameof(Resources.Links.LinkDto.linkUrl), ResourceType = typeof(Resources.Links.LinkDto))]
        public Uri? LinkUrl { get; set; }
        [Display(Name = nameof(Resources.Links.LinkDto.title), ResourceType = typeof(Resources.Links.LinkDto))]
        public string Title { get; set; } = string.Empty;
        [Display(Name = nameof(Resources.Links.LinkDto.description), ResourceType = typeof(Resources.Links.LinkDto))]
        public string? Description { get; set; }
        [Display(Name = nameof(Resources.Links.LinkDto.privacy), ResourceType = typeof(Resources.Links.LinkDto))]
        public LinkPrivacyOptions PrivacyOptions { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdate { get; set; }

        [SuppressMessage("Design", "CA1002:Do not expose generic lists")]
        public List<TagDto> LinkTags { get; } = new();
    }

    public enum LinkPrivacyOptions
    {
        [Display(Name = nameof(Resources.Links.LinkDto.enumPrivate), ResourceType = typeof(Resources.Links.LinkDto))]
        Private,
        [Display(Name = nameof(Resources.Links.LinkDto.enumPublic), ResourceType = typeof(Resources.Links.LinkDto))]
        Public
    }
}
