using System;
using Rinkudesu.Gateways.Clients.Links;

namespace Rinkudesu.Gateways.Webui.Models
{
    public class LinkIndexViewModel
    {
        public Guid Id { get; set; }
        public string LinkUrl { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public LinkPrivacyOptions PrivacyOptions { get; set; }
    }
}