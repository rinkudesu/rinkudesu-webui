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
using Rinkudesu.Gateways.Utils;
using Rinkudesu.Gateways.Webui.Utils;

namespace Rinkudesu.Gateways.Webui.Controllers
{
    [Authorize]
    public class LinksController : Controller
    {
        private readonly IStringLocalizer<LinksController> _localizer;
        private readonly LinksClient _client;
        private readonly IMapper _mapper;

        public LinksController(IMapper mapper, LinksClient client, IStringLocalizer<LinksController> localizer)
        {
            _mapper = mapper;
            _client = client;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var accessToken = await HttpContext.GetJwt();
            var links = await _client.SetAccessToken(accessToken).GetLinks();
            if (links is null)
                return this.ReturnNotFound("/".ToUri());
            return View(_mapper.Map<List<LinkDto>>(links.ToList()));
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind] LinkDto newLink)
        {
            if (!ModelState.IsValid)
                return this.ReturnBadRequest(Url.ActionLink(nameof(Index))!.ToUri());

            var jwt = await HttpContext.GetJwt();
            var isSuccess = await _client.SetAccessToken(jwt).CreateLink(newLink);
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

            var jwt = await HttpContext.GetJwt();
            var isSuccess = await _client.SetAccessToken(jwt).CreateLink(newLink);
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

            var jwt = await HttpContext.GetJwt();
            if (!await _client.SetAccessToken(jwt).Delete(guid))
                return this.ReturnBadRequest(Url.ActionLink(nameof(Index))!.ToUri(), _localizer["unableToDelete"]);

            return LocalRedirect(returnUrl.ToString());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id, Uri returnUrl, CancellationToken token)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var guid))
                return this.ReturnBadRequest(Url.ActionLink(nameof(Index))!.ToUri(), _localizer["invalidId"]);

            var jwt = await HttpContext.GetJwt();
            var link = await _client.SetAccessToken(jwt).GetLink(guid, token);

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

            var jwt = await HttpContext.GetJwt();
            var result = await _client.SetAccessToken(jwt).Edit(guid, editLink);

            if (!result)
                return this.ReturnNotFound(Url.ActionLink(nameof(Index))!.ToUri());
            return LocalRedirect(returnUrl.ToString());
        }
    }
}
