using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Rinkudesu.Gateways.Webui.Models;

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
        public IActionResult Privacy()
        {
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