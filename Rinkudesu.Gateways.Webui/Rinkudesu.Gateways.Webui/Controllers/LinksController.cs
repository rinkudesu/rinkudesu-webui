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
using Rinkudesu.Gateways.Webui.Models;
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
        public IActionResult Index([FromQuery] LinkIndexQueryModel query)
        {
            return View(query);
        }

        [HttpGet]
        public async Task<ActionResult> IndexContent([FromQuery] LinkIndexQueryModel query, [FromServices] LinkTagsClient linkTagsClient, Uri returnUrl, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return BadRequest("Provided query is not valid.");

            await linkTagsClient.SetAccessToken(HttpContext.Request);
            await SetJwt();
            var links = await Client.GetLinks(_mapper.Map<LinkQueryDto>(query), cancellationToken);
            if (links is null)
                return NotFound();
            var linkModels = _mapper.Map<List<LinkIndexViewModel>>(links.ToList());

            foreach (var link in linkModels)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var tags = await linkTagsClient.GetTagsForLink(link.Id, cancellationToken);
                if (tags is null) return BadRequest("We were unable to process tags assigned to links.");
                link.LinkTags.AddRange(tags);
            }

            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Query"] = query;
            return PartialView(linkModels);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind] LinkIndexViewModel newLink, [FromServices] LinkTagsClient linkTagsClient)
        {
            if (!ModelState.IsValid)
                return this.ReturnBadRequest(Url.ActionLink(nameof(Index))!.ToUri());

            var newLinkDto = _mapper.Map<LinkDto>(newLink);

            await SetJwt();
            var returnedDto = await Client.CreateLink(newLinkDto);
            if (returnedDto is null)
                return this.ReturnBadRequest(Url.ActionLink(nameof(Index))!.ToUri(), _localizer["unableToCreate"]);
            await linkTagsClient.SetAccessToken(HttpContext.Request);
            foreach (var tagId in newLink.TagIds)
            {
                //todo: consider some sort of error handling here, as the link already exists at this point, but just quietly failing to create tags is not the best option
                await linkTagsClient.Assign(new() { LinkId = returnedDto.Id, TagId = tagId });
            }
            return Redirect(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuickCreate(Uri? url)
        {
            if (url is null)
                return this.ReturnBadRequest(Url.ActionLink(nameof(Index))!.ToUri(), _localizer["missingUrl"]);

            var newLink = new LinkDto { Title = url.ToString(), LinkUrl = url, PrivacyOptions = LinkPrivacyOptionsDto.Private };

            await SetJwt();
            var isSuccess = await Client.CreateLink(newLink) is not null;
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

            await SetJwt();
            if (!await Client.Delete(guid))
                return this.ReturnBadRequest(Url.ActionLink(nameof(Index))!.ToUri(), _localizer["unableToDelete"]);

            return LocalRedirect(returnUrl.ToString());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id, Uri returnUrl, CancellationToken token)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var guid))
                return this.ReturnBadRequest(Url.ActionLink(nameof(Index))!.ToUri(), _localizer["invalidId"]);

            await SetJwt();
            var link = await Client.GetLink(guid, token);

            if (link is null)
                return this.ReturnNotFound(Url.ActionLink(nameof(Index))!.ToUri());
            ViewData["ReturnUrl"] = returnUrl;
            return View(_mapper.Map<LinkIndexViewModel>(link));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string? id, [Bind] LinkDto editLink, Uri returnUrl)
        {
            if (!Guid.TryParse(id, out var guid) || !ModelState.IsValid)
                return this.ReturnBadRequest(Url.ActionLink(nameof(Index))!.ToUri(), _localizer["invalidId"]);

            await SetJwt();
            var result = await Client.Edit(guid, editLink);

            if (!result)
                return this.ReturnNotFound(Url.ActionLink(nameof(Index))!.ToUri());
            return LocalRedirect(returnUrl.ToString());
        }
    }
}
