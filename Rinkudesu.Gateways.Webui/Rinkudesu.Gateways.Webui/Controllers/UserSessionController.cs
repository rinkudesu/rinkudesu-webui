using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rinkudesu.Gateways.MessageQueues;
using Rinkudesu.Gateways.Utils;
using Rinkudesu.Gateways.Webui.Models;
using Rinkudesu.Gateways.Webui.Utils;
using Rinkudesu.Kafka.Dotnet.Base;

namespace Rinkudesu.Gateways.Webui.Controllers
{
    public class UserSessionController : Controller
    {

// Disable antiforgery token check
#pragma warning disable CA5391
        [HttpGet]
        [HttpPost]
        [Authorize] //This function only redirects, but since the authorize attribute is set, a login flow will be triggered if user is not logged in
        public IActionResult Login(Uri? returnUrl)
        {
            return LocalRedirect(returnUrl?.ToString() ?? "/");
        }
#pragma warning restore CA5391

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            if (!User.Identity!.IsAuthenticated) return Redirect("/");
            await HttpContext.SignOutAsync();
            return Redirect($"{KeycloakSettings.Current.Authority}/protocol/openid-connect/logout?client_id={KeycloakSettings.Current.ClientId}&post_logout_redirect_uri={HttpContext.GetEncodedBasePath()}");
        }
    }
}
