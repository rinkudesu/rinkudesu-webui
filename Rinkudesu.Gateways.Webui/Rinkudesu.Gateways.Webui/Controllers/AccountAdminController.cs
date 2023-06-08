using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rinkudesu.Gateways.Clients.Identity;
using Rinkudesu.Gateways.Webui.Utils;

namespace Rinkudesu.Gateways.Webui.Controllers;

[Authorize (Roles = Roles.Admin)]
public class AccountAdminController : Controller
{
    private readonly IdentityClient _identityClient;

    public AccountAdminController(IdentityClient identityClient)
    {
        _identityClient = identityClient;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var users = await _identityClient.ReadIdentityCookie(Request).GetUsersAdmin();
        //todo: handle null
        //todo: proper pagination
        return View(users);
    }
}
