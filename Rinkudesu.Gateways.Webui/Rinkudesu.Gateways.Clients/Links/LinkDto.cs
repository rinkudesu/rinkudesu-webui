using System;
using System.ComponentModel.DataAnnotations;

namespace Rinkudesu.Gateways.Clients.Links
{
    public class LinkDto
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
    }

    public enum LinkPrivacyOptions
    {
        [Display(Name = nameof(Resources.Links.LinkDto.enumPrivate), ResourceType = typeof(Resources.Links.LinkDto))]
        Private,
        [Display(Name = nameof(Resources.Links.LinkDto.enumPublic), ResourceType = typeof(Resources.Links.LinkDto))]
        Public
    }
}
