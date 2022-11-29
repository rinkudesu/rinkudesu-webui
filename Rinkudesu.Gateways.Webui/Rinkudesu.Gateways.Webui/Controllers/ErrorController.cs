using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
#pragma warning disable CA5391

namespace Rinkudesu.Gateways.Webui.Controllers;

[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class ErrorController : Controller
{
    public const string ERROR_DETAILS_ITEM_NAME = "ErrorDetails";
    public const string RETURN_URL_ITEM_NAME = "ReturnUrl";

    [HttpGet, HttpPost]
    [IgnoreAntiforgeryToken]
    public IActionResult Error404() => View();

    [HttpGet, HttpPost]
    [IgnoreAntiforgeryToken]
    public IActionResult Error400() => View();
}
