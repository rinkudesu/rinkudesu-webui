using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rinkudesu.Gateways.Clients.Links;
using Rinkudesu.Gateways.Utils;
using Rinkudesu.Gateways.Webui.Utils;

namespace Rinkudesu.Gateways.Webui.Controllers;

[Authorize]
public class SharedLinksAccessController : AccessTokenClientController<LinksClient>
{
    public SharedLinksAccessController(LinksClient client) : base(client)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Get(string key, CancellationToken cancellationToken)
    {
        var link = await Client.GetLink(key, cancellationToken);

        if (link is null)
            return this.ReturnNotFound(Url.ActionLink(nameof(Index), "Links")!.ToUri());
        return View(link);
    }
}
