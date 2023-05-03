using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rinkudesu.Gateways.Clients.Links;
using Rinkudesu.Gateways.Webui.Utils;

namespace Rinkudesu.Gateways.Webui.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[ExcludeFromCodeCoverage]
public class SharedLinksController : AccessTokenClientControllerBase<SharedLinksClient>
{
    public SharedLinksController(SharedLinksClient client) : base(client)
    {
    }

    [HttpGet("shared/{id:guid}")]
    public async Task<ActionResult<bool>> IsShared(Guid id, CancellationToken cancellationToken)
    {
        return Ok(await Client.IsShared(id, cancellationToken));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<string>> GetKey(Guid id, CancellationToken cancellationToken)
    {
        await SetJwt();
        return Ok(await Client.GetKey(id, cancellationToken));
    }

    [HttpPost("{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<string>> Share(Guid id, CancellationToken cancellationToken)
    {
        await SetJwt();
        return Ok(await Client.Share(id, cancellationToken));
    }

    [HttpDelete("{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Unshare(Guid id, CancellationToken cancellationToken)
    {
        await SetJwt();
        await Client.Unshare(id, cancellationToken);
        return Ok();
    }
}
