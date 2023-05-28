using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rinkudesu.Gateways.Clients.Identity;
using Rinkudesu.Gateways.MessageQueues;
using Rinkudesu.Gateways.Utils;
using Rinkudesu.Gateways.Webui.Models;
using Rinkudesu.Gateways.Webui.Utils;
using Rinkudesu.Kafka.Dotnet.Base;

namespace Rinkudesu.Gateways.Webui.Controllers
{
    public class UserSessionController : Controller
    {
        private readonly IdentityClient _identityClient;
        private readonly IStringLocalizer<UserSessionController> _localizer;
        private readonly IMapper _mapper;

        public UserSessionController(IdentityClient identityClient, IStringLocalizer<UserSessionController> localizer, IMapper mapper)
        {
            _identityClient = identityClient;
            _localizer = localizer;
            _mapper = mapper;
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

        [HttpGet]
        public IActionResult ForgotPassword() => View(new ForgotPasswordViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword([Bind] ForgotPasswordViewModel model, [FromServices] IKafkaProducer kafka, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return this.ReturnBadRequest("/".ToUri());

            var token = await _identityClient.ForgotPassword(_mapper.Map<ForgotPasswordDto>(model), cancellationToken: cancellationToken);
            // never respond with error here, as that could allow attackers to enumerate users
            if (token is not null)
            {
                await kafka.ProduceSendEmail(
                    token.UserId,
                    _localizer["forgotPasswordSubject"],
                    _localizer["forgotPasswordIntro"] + $"<br/><a href='{HttpContext.GetBasePath()}{Url.Action(nameof(ChangeForgottenPassword), new { token.UserId, token.Token })!.TrimStart('/')}'>{_localizer["forgotPasswordClick"]}</a>",
                    true);
            }

            ViewData["Submitted"] = "1";
            return View(new ForgotPasswordViewModel());
        }

        [HttpGet]
        public IActionResult ChangeForgottenPassword(Guid userId, string token)
            => View(new ChangeForgottenPasswordViewModel
            {
                Token = token,
                UserId = userId,
            });

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeForgottenPassword([Bind] ChangeForgottenPasswordViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return this.ReturnBadRequest("/".ToUri());
            if (model.PasswordMismatch)
                return this.ReturnBadRequest(Url.Action(nameof(ChangeForgottenPassword), new { model.UserId, model.Token })!.ToUri(), _localizer["passwordMismatch"]);

            if (await _identityClient.ResetPassword(_mapper.Map<ChangeForgottenPasswordDto>(model), cancellationToken))
                return RedirectToAction(nameof(Login));

            return this.ReturnBadRequest("/".ToUri());
        }
    }
}
