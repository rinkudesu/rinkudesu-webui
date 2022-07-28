using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Rinkudesu.Gateways.Clients.Tags;
using Rinkudesu.Gateways.Webui.Models;
using Rinkudesu.Gateways.Webui.Utils;

namespace Rinkudesu.Gateways.Webui.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Privacy([FromServices] TagsClient client)
        {
            await client.SetAccessToken(await HttpContext.GetJwt()).ThisIsATestPleaseIgnore();
            return View();
        }

// Disable antiforgery token check
#pragma warning disable CA5391
        [HttpGet]
        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
#pragma warning restore CA5391
    }
}
