using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rinkudesu.Gateways.Clients.Identity;
using Rinkudesu.Gateways.Utils;
using Rinkudesu.Gateways.Webui.Models;
using Rinkudesu.Gateways.Webui.Utils;

namespace Rinkudesu.Gateways.Webui.Controllers
{
    public class UserSessionController : Controller
    {
        private readonly IdentityClient _identityClient;
        private readonly IStringLocalizer<UserSessionController> _localizer;

        public UserSessionController(IdentityClient identityClient, IStringLocalizer<UserSessionController> localizer)
        {
            _identityClient = identityClient;
            _localizer = localizer;
        }

        [HttpGet]
        public IActionResult Login(Uri? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl ?? "/".ToUri();
            return View(new UserLoginViewModel());
        }

        [HttpPost("login"), ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginPost([Bind] UserLoginViewModel userLogin, Uri? returnUrl)
        {
            returnUrl ??= new Uri("/", UriKind.Relative);
            if (!ModelState.IsValid)
                return this.ReturnBadRequest(returnUrl);
            if (!await _identityClient.PostLogin(HttpContext.Response, userLogin.UserName, userLogin.Password))
                return this.ReturnNotFound(Url.ActionLink(nameof(Login), values: new { returnUrl })!.ToUri(), _localizer["login-failed"]);

            return LocalRedirect(returnUrl.ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            if (!User.Identity!.IsAuthenticated) return Redirect("/");
            await _identityClient.ReadIdentityCookie(HttpContext.Request).PostLogOut();
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
