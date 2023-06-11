using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rinkudesu.Gateways.Clients.Identity;
using Rinkudesu.Gateways.Webui.Models;
using Rinkudesu.Gateways.Webui.Utils;

namespace Rinkudesu.Gateways.Webui.Controllers;

[Authorize (Roles = Roles.Admin)]
public class AccountAdminController : Controller
{
    private readonly IdentityClient _identityClient;
    private readonly IMapper _mapper;

    public AccountAdminController(IdentityClient identityClient, IMapper mapper)
    {
        _identityClient = identityClient;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult Index([FromQuery] UserAdminIndexQueryViewModel query)
    {
        return View(query);
    }

    [HttpGet]
    public async Task<ActionResult> IndexContent([FromQuery] UserAdminIndexQueryViewModel query, Uri returnUrl, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest("Provided query is not valid.");

        var users = await _identityClient.ReadIdentityCookie(Request).GetUsersAdmin(_mapper.Map<UserAdminIndexQueryDto>(query), cancellationToken: cancellationToken);
        if (users is null)
            return BadRequest("Failed to load users");

        ViewData["ReturnUrl"] = returnUrl;
        ViewData["Query"] = query;
        return PartialView(_mapper.Map<List<UserAdminDetailsViewModel>>(users));
    }
}
