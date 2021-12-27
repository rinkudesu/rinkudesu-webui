using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rinkudesu.Gateways.Clients.Links;
using Rinkudesu.Gateways.Webui.Utils;

namespace Rinkudesu.Gateways.Webui.Controllers
{
    [Authorize]
    public class LinksController : Controller
    {
        private readonly LinksClient _client;
        private readonly IMapper _mapper;

        public LinksController(IMapper mapper, LinksClient client)
        {
            _mapper = mapper;
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var accessToken = await HttpContext.GetJwt();
            var links = await _client.SetAccessToken(accessToken).GetLinks();
            if (links is null)
            {
                return NotFound(); //TODO: make this prettier
            }
            return View(_mapper.Map<List<LinkDto>>(links.ToList()));
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind] LinkDto newLink)
        {
            if (!ModelState.IsValid) return BadRequest(); //TODO: make this prettier

            var jwt = await HttpContext.GetJwt();
            var isSuccess = await _client.SetAccessToken(jwt).CreateLink(newLink);
            if (!isSuccess) return BadRequest(); //TODO: some display for error would be nice here
            return Redirect(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuickCreate(Uri? url)
        {
            if (url is null) return BadRequest(); //TODO: make this prettier

            var newLink = new LinkDto { Title = url.ToString(), LinkUrl = url, PrivacyOptions = LinkPrivacyOptions.Private };

            var jwt = await HttpContext.GetJwt();
            var isSuccess = await _client.SetAccessToken(jwt).CreateLink(newLink);
            if (!isSuccess) return BadRequest(); //TODO: some display for error would be nice here
            return Redirect(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string? id, Uri returnUrl)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();
            if (!Guid.TryParse(id, out var guid)) return BadRequest();

            var jwt = await HttpContext.GetJwt();
            if (!await _client.SetAccessToken(jwt).Delete(guid)) return BadRequest();

            return LocalRedirect(returnUrl.ToString());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id, Uri returnUrl, CancellationToken token)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var guid)) return BadRequest();

            var jwt = await HttpContext.GetJwt();
            var link = await _client.SetAccessToken(jwt).GetLink(guid, token);

            if (link is null) return NotFound(); //todo: some display error
            ViewData["ReturnUrl"] = returnUrl;
            return View(link);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string? id, [Bind] LinkDto editLink, Uri returnUrl)
        {
            if (!Guid.TryParse(id, out var guid) || !ModelState.IsValid) return BadRequest(); //todo: prettier

            var jwt = await HttpContext.GetJwt();
            var result = await _client.SetAccessToken(jwt).Edit(guid, editLink);

            if (!result) return NotFound(); //todo: prettier
            return LocalRedirect(returnUrl.ToString());
        }
    }
}