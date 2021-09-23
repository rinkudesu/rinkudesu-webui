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
    }
}