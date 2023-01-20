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
        public Uri? LinkUrl { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public LinkPrivacyOptionsDto PrivacyOptions { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdate { get; set; }
    }

    public enum LinkPrivacyOptionsDto
    {
        Private,
        Public
    }
}
