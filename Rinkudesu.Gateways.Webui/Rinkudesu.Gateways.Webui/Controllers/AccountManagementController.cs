using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
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
    private readonly IStringLocalizer<AccountManagementController> _localizer;

    public AccountManagementController(IdentityClient client, IMapper mapper, IStringLocalizer<AccountManagementController> localizer)
    {
        _client = client;
        _mapper = mapper;
        _localizer = localizer;
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
            return this.ReturnBadRequest(returnUrl, _localizer["invalidForm"]);
        if (!model.NewPasswordsMatch)
            return this.ReturnBadRequest(returnUrl, _localizer["passwordMismatch"]);

        var changed = await _client.ReadIdentityCookie(Request).ChangePassword(_mapper.Map<PasswordChangeDto>(model));

        if (!changed)
            return this.ReturnBadRequest(returnUrl, _localizer["failedToChange"]);

        return RedirectToAction(nameof(Index), new { isSuccess = true });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> LogOutEverywhere()
    {
        if (!await _client.ReadIdentityCookie(Request).LogOutEverywhere())
            return this.ReturnBadRequest("/".ToUri());

        // if logger out, just redirect to home to avoid immediate login screens
        return LocalRedirect("/");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAccount([Bind] DeleteAccountViewModel model)
    {
        if (!ModelState.IsValid)
            return this.ReturnBadRequest("/".ToUri());

        var result = await _client.ReadIdentityCookie(Request).DeleteAccount(_mapper.Map<AccountDeleteDto>(model));

        return result ? LocalRedirect("/") : this.ReturnBadRequest(Url.ActionLink(nameof(Index))!.ToUri());
    }
}
