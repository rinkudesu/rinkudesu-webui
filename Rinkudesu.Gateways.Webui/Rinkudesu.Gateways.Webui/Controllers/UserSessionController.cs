using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Rinkudesu.Gateways.Webui.Utils;

namespace Rinkudesu.Gateways.Webui.Controllers
{
    public class UserSessionController : Controller
    {
        [HttpGet]
        [HttpPost]
        [Authorize] //This function only redirects, but since the authorize attribute is set, a login flow will be triggered if user is not logged in
        public IActionResult Login(string returnUrl) => Redirect(returnUrl ?? "/");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            if (!User.Identity.IsAuthenticated) return Redirect("/");
            await HttpContext.SignOutAsync();
            return Redirect($"http://localhost:8080/auth/realms/rinkudesu/protocol/openid-connect/logout?redirect_uri={HttpContext.GetEncodedBasePath()}"); //TODO: read from file
        }
    }
}