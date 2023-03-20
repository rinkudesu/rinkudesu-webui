using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Rinkudesu.Gateways.Clients.Tags;
using Rinkudesu.Gateways.Utils;

namespace Rinkudesu.Gateways.Webui.Models
{
    public class LinkIndexViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = nameof(Resources.Models.Links.LinkIndexViewModel.linkUrl), ResourceType = typeof(Resources.Models.Links.LinkIndexViewModel))]
        public Uri LinkUrl { get; set; } = string.Empty.ToUri();
        [Display(Name = nameof(Resources.Models.Links.LinkIndexViewModel.title), ResourceType = typeof(Resources.Models.Links.LinkIndexViewModel))]
        public string Title { get; set; } = string.Empty;
        [Display(Name = nameof(Resources.Models.Links.LinkIndexViewModel.description), ResourceType = typeof(Resources.Models.Links.LinkIndexViewModel))]
        public string? Description { get; set; }
        [Display(Name = nameof(Resources.Models.Links.LinkIndexViewModel.privacy), ResourceType = typeof(Resources.Models.Links.LinkIndexViewModel))]
        public LinkPrivacyOptions PrivacyOptions { get; set; }

        [SuppressMessage("Design", "CA1002:Do not expose generic lists")]
        public List<TagDto> LinkTags { get; } = new();
        /// <summary>
        /// This is a list of tag ids used only during link creation.
        /// </summary>
        [SuppressMessage("Design", "CA1002:Do not expose generic lists")]
        [Display(Name = nameof(Resources.Models.Links.LinkIndexViewModel.tagIds), ResourceType = typeof(Resources.Models.Links.LinkIndexViewModel))]
        public List<Guid> TagIds { get; } = new();
    }

    public enum LinkPrivacyOptions
    {
        [Display(Name = nameof(Resources.Models.Links.LinkIndexViewModel.enumPrivate), ResourceType = typeof(Resources.Models.Links.LinkIndexViewModel))]
        Private,
        [Display(Name = nameof(Resources.Models.Links.LinkIndexViewModel.enumPublic), ResourceType = typeof(Resources.Models.Links.LinkIndexViewModel))]
        Public
    }
}
