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
public class SharedLinksController : ControllerBase
{
    private readonly SharedLinksClient _client;

    public SharedLinksController(SharedLinksClient client)
    {
        _client = client;
    }

    [HttpGet("shared/{id:guid}")]
    public async Task<ActionResult<bool>> IsShared(Guid id, CancellationToken cancellationToken)
    {
        var token = await HttpContext.GetJwt();
        return Ok(await _client.SetAccessToken(token).IsShared(id, cancellationToken));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<string>> GetKey(Guid id, CancellationToken cancellationToken)
    {
        var token = await HttpContext.GetJwt();
        return Ok(await _client.SetAccessToken(token).GetKey(id, cancellationToken));
    }

    [HttpPost("{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<string>> Share(Guid id, CancellationToken cancellationToken)
    {
        var token = await HttpContext.GetJwt();
        return Ok(await _client.SetAccessToken(token).Share(id, cancellationToken));
    }

    [HttpDelete("{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Unshare(Guid id, CancellationToken cancellationToken)
    {
        var token = await HttpContext.GetJwt();
        await _client.SetAccessToken(token).Unshare(id, cancellationToken);
        return Ok();
    }
}