using System;
using System.ComponentModel.DataAnnotations;

namespace Rinkudesu.Gateways.Clients.Links
{
    public class LinkDto
    {
        public Guid Id { get; set; }
        [Display(Name = "Link url")]
        public Uri? LinkUrl { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Display(Name = "Privacy options")]
        public LinkPrivacyOptions PrivacyOptions { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdate { get; set; }
    }

    public enum LinkPrivacyOptions
    {
        Private,
        Public
    }
}