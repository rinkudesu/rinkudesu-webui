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
        private readonly IKafkaProducer _kafkaProducer;

        public UserSessionController(IKafkaProducer kafkaProducer)
        {
            _kafkaProducer = kafkaProducer;
        }

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
            return Redirect($"{KeycloakSettings.Current.Authority}/protocol/openid-connect/logout?redirect_uri={HttpContext.GetEncodedBasePath()}");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser()
        {
            //todo: this is just a mock, it just posts "delete user" message to queue, but doesn't actually delete any users
            await _kafkaProducer.ProduceUserDeleted(User.GetIdAsGuid());
            return LocalRedirect("/");
        }
    }
}
