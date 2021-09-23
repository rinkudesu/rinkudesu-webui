using System;

namespace Rinkudesu.Gateways.Clients.Links
{
    public class LinkDto
    {
        public Guid Id { get; set; }
        public string LinkUrl { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public LinkPrivacyOptions PrivacyOptions { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public string CreatingUserId { get; set; } = string.Empty;
    }

    public enum LinkPrivacyOptions
    {
        Private,
        Public
    }
}