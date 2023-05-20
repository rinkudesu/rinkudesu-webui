using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rinkudesu.Gateways.Clients.Identity;
using Rinkudesu.Gateways.Utils;
using Rinkudesu.Gateways.Webui.Models;
using Rinkudesu.Gateways.Webui.Utils;

namespace Rinkudesu.Gateways.Webui.Controllers;

[Authorize]
public class AccountManagementController : Controller
{
    private readonly IdentityClient _client;
    private readonly IMapper _mapper;

    public AccountManagementController(IdentityClient client, IMapper mapper)
    {
        _client = client;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Index(bool isSuccess = false)
    {
        var details = await _client.ReadIdentityCookie(Request).GetDetails();
        if (details is null)
            return this.ReturnNotFound("/".ToUri());

        ViewData["IsSuccess"] = isSuccess;
        return View(details);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword([Bind] PasswordChangeViewModel model)
    {
        var returnUrl = Url.ActionLink(nameof(Index))!.ToUri();
        if (!ModelState.IsValid)
            return this.ReturnBadRequest(returnUrl);//todo: display localised error
        if (!model.NewPasswordsMatch)
            return this.ReturnBadRequest(returnUrl);//todo: display localised error

        var changed = await _client.ReadIdentityCookie(Request).ChangePassword(_mapper.Map<PasswordChangeDto>(model));

        if (!changed)
            return this.ReturnBadRequest(returnUrl);//todo: display localised error

        return RedirectToAction(nameof(Index), new { isSuccess = true });
    }
}
