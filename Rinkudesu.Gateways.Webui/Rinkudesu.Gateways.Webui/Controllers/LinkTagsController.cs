using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rinkudesu.Gateways.Clients.LinkTags;
using Rinkudesu.Gateways.Webui.Models;

namespace Rinkudesu.Gateways.Webui.Controllers;

[ApiController, Route("api/[controller]"), Authorize, ExcludeFromCodeCoverage]
public class LinkTagsController : AccessTokenClientController<LinkTagsClient>
{
    private readonly IMapper _mapper;

    public LinkTagsController(LinkTagsClient client, IMapper mapper) : base(client)
    {
        _mapper = mapper;
    }

    [HttpGet("getTagsForLink/{id:guid}")]
    public async Task<IActionResult> GetTagsForLink(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            return BadRequest();

        var tags = await Client.GetTagsForLink(id, cancellationToken);

        ViewData["LinkId"] = id;
        if (tags is null)
            return NotFound();
        return PartialView(_mapper.Map<List<TagIndexViewModel>>(tags));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Assign([FromForm] LinkTagDto newLinkTag, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var result = await Client.Assign(newLinkTag, cancellationToken);

        return result ? Ok() : BadRequest();
    }

    [HttpPost("delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete([FromForm] Guid linkId, [FromForm] Guid tagId, CancellationToken cancellationToken)
    {
        if (linkId == Guid.Empty || tagId == Guid.Empty)
            return BadRequest();

        var result = await Client.Delete(linkId, tagId, cancellationToken);

        return result ? Ok() : BadRequest();
    }
}
