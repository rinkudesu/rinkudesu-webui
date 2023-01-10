using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rinkudesu.Gateways.Clients.Links;
using Rinkudesu.Gateways.Clients.LinkTags;
using Rinkudesu.Gateways.Utils;
using Rinkudesu.Gateways.Webui.Utils;

namespace Rinkudesu.Gateways.Webui.Controllers
{
    [Authorize]
    public class LinksController : AccessTokenClientController<LinksClient>
    {
        private readonly IStringLocalizer<LinksController> _localizer;
        private readonly IMapper _mapper;

        public LinksController(IMapper mapper, LinksClient client, IStringLocalizer<LinksController> localizer) : base(client)
        {
            _mapper = mapper;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromServices] LinkTagsClient linkTagsClient, CancellationToken cancellationToken)
        {
            linkTagsClient.SetAccessToken(HttpContext.GetJwt());
            var links = await Client.GetLinks(cancellationToken);
            if (links is null)
                return this.ReturnNotFound("/".ToUri());
            var linkModels = _mapper.Map<List<LinkDto>>(links.ToList());

            foreach (var link in linkModels)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var tags = await linkTagsClient.GetTagsForLink(link.Id, cancellationToken);
                if (tags is null) return this.ReturnBadRequest("/".ToUri(), "We were unable to process tags assigned to links.");
                link.LinkTags = tags.ToList();
            }

            return View(linkModels);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind] LinkDto newLink)
        {
            if (!ModelState.IsValid)
                return this.ReturnBadRequest(Url.ActionLink(nameof(Index))!.ToUri());

            var isSuccess = await Client.CreateLink(newLink);
            if (!isSuccess)
                this.ReturnBadRequest(Url.ActionLink(nameof(Index))!.ToUri(), _localizer["unableToCreate"]);
            return Redirect(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuickCreate(Uri? url)
        {
            if (url is null)
                return this.ReturnBadRequest(Url.ActionLink(nameof(Index))!.ToUri(), _localizer["missingUrl"]);

            var newLink = new LinkDto { Title = url.ToString(), LinkUrl = url, PrivacyOptions = LinkPrivacyOptions.Private };

            var isSuccess = await Client.CreateLink(newLink);
            if (!isSuccess)
                this.ReturnBadRequest(Url.ActionLink(nameof(Index))!.ToUri(), _localizer["unableToCreate"]);
            return Redirect(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string? id, Uri returnUrl)
        {
            if (string.IsNullOrEmpty(id))
                return this.ReturnBadRequest(Url.ActionLink(nameof(Index))!.ToUri(), _localizer["missingId"]);
            if (!Guid.TryParse(id, out var guid))
                return this.ReturnBadRequest(Url.ActionLink(nameof(Index))!.ToUri(), _localizer["invalidId"]);

            if (!await Client.Delete(guid))
                return this.ReturnBadRequest(Url.ActionLink(nameof(Index))!.ToUri(), _localizer["unableToDelete"]);

            return LocalRedirect(returnUrl.ToString());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id, Uri returnUrl, CancellationToken token)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var guid))
                return this.ReturnBadRequest(Url.ActionLink(nameof(Index))!.ToUri(), _localizer["invalidId"]);

            var link = await Client.GetLink(guid, token);

            if (link is null)
                return this.ReturnNotFound(Url.ActionLink(nameof(Index))!.ToUri());
            ViewData["ReturnUrl"] = returnUrl;
            return View(link);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string? id, [Bind] LinkDto editLink, Uri returnUrl)
        {
            if (!Guid.TryParse(id, out var guid) || !ModelState.IsValid)
                return this.ReturnBadRequest(Url.ActionLink(nameof(Index))!.ToUri(), _localizer["invalidId"]);

            var result = await Client.Edit(guid, editLink);

            if (!result)
                return this.ReturnNotFound(Url.ActionLink(nameof(Index))!.ToUri());
            return LocalRedirect(returnUrl.ToString());
        }
    }
}
