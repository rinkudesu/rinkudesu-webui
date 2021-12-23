using System;
using Rinkudesu.Gateways.Clients.Links;
using Rinkudesu.Gateways.Utils;

namespace Rinkudesu.Gateways.Webui.Models
{
    public class LinkIndexViewModel
    {
        public Guid Id { get; set; }
        public Uri LinkUrl { get; set; } = string.Empty.ToUri();
        public string Title { get; set; } = string.Empty;
        public LinkPrivacyOptions PrivacyOptions { get; set; }
    }
}