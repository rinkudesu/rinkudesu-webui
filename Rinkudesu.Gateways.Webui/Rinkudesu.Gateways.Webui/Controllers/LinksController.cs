using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rinkudesu.Gateways.Clients.Links;

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
            var links = await _client.GetLinks();
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

            var isSuccess = await _client.CreateLink(newLink);
            if (!isSuccess) return BadRequest(); //TODO: some display for error would be nice here
            return Redirect(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuickCreate(string? url)
        {
            if (string.IsNullOrEmpty(url)) return BadRequest(); //TODO: make this prettier

            var newLink = new LinkDto { Title = url, LinkUrl = url, PrivacyOptions = LinkPrivacyOptions.Private };

            var isSuccess = await _client.CreateLink(newLink);
            if (!isSuccess) return BadRequest(); //TODO: some display for error would be nice here
            return Redirect(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string? id, string? returnUrl)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();
            if (!Guid.TryParse(id, out var guid)) return BadRequest();

            if (!await _client.Delete(guid)) return BadRequest();

            if (returnUrl is null || !returnUrl.StartsWith('/')) returnUrl = Url.Action(nameof(Index));
            return Redirect(returnUrl);
        }
    }
}