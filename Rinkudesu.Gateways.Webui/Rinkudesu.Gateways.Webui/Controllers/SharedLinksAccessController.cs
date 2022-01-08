using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rinkudesu.Gateways.Clients.Links;
using Rinkudesu.Gateways.Webui.Utils;

namespace Rinkudesu.Gateways.Webui.Controllers;

[Authorize]
public class SharedLinksAccessController : Controller
{
    private readonly LinksClient _client;

    public SharedLinksAccessController(LinksClient client)
    {
        _client = client;
    }

    [HttpGet]
    public async Task<IActionResult> Get(string key, CancellationToken cancellationToken)
    {
        var jwt = await HttpContext.GetJwt();
        var link = await _client.SetAccessToken(jwt).GetLink(key, cancellationToken);

        if (link is null) return NotFound(); //todo: some display error
        return View(link);
    }
}