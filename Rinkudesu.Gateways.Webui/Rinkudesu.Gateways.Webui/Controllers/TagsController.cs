﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rinkudesu.Gateways.Clients.Tags;
using Rinkudesu.Gateways.Utils;
using Rinkudesu.Gateways.Webui.Models;
using Rinkudesu.Gateways.Webui.Utils;

namespace Rinkudesu.Gateways.Webui.Controllers;

[Authorize]
public class TagsController : AccessTokenClientController<TagsClient>
{
    private readonly IMapper _mapper;
    public TagsController(TagsClient client, IMapper mapper) : base(client)
    {
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult Index([Bind] TagIndexQueryModel query)
    {
        return View(query);
    }

    [HttpGet]
    public async Task<ActionResult> IndexContent([Bind] TagIndexQueryModel query, Uri returnUrl, CancellationToken cancellationToken)
    {
        await SetJwt();
        var tags = await Client.GetTags(_mapper.Map<TagQueryDto>(query), cancellationToken);
        if (tags is null)
            return NotFound();

        ViewData["ReturnUrl"] = returnUrl;
        ViewData["Query"] = query;
        return PartialView(_mapper.Map<List<TagIndexViewModel>>(tags));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind] TagDto newTag)
    {
        if (!ModelState.IsValid)
            return this.ReturnBadRequest(Url.ActionLink(nameof(Index))!.ToUri());

        await SetJwt();
        var isSuccess = await Client.CreateTag(newTag) is not null;
        if (!isSuccess)
            return this.ReturnBadRequest(Url.ActionLink(nameof(Index))!.ToUri());
        return Redirect(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, Uri returnUrl, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            return this.ReturnBadRequest(returnUrl);

        await SetJwt();
        var tag = await Client.GetTag(id, cancellationToken);

        if (tag is null)
            return this.ReturnNotFound(returnUrl);
        ViewData["ReturnUrl"] = returnUrl;
        return View(tag);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind] TagDto editTag, Uri returnUrl, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty || !ModelState.IsValid)
            return this.ReturnBadRequest(returnUrl);

        await SetJwt();
        var result = await Client.Edit(id, editTag, cancellationToken);

        if (!result)
            return this.ReturnNotFound(returnUrl);
        return LocalRedirect(returnUrl.ToString());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, Uri returnUrl, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            return this.ReturnBadRequest(returnUrl);

        await SetJwt();
        var result = await Client.Delete(id, cancellationToken);

        if (!result)
            return this.ReturnBadRequest(returnUrl);
        return LocalRedirect(returnUrl.ToString());
    }
}
