using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace Rinkudesu.Gateways.Webui.Utils;

[ExcludeFromCodeCoverage]
public static class ControllerExtensions
{
    public static IActionResult ReturnNotFound(this ControllerBase controller, Uri redirectUri, string? errorDetails = null)
    {
        controller.HttpContext.AddErrorDetails(redirectUri, errorDetails);
        return controller.NotFound();
    }

    public static IActionResult ReturnBadRequest(this ControllerBase controller, Uri redirectUri, string? errorDetails = null)
    {
        controller.HttpContext.AddErrorDetails(redirectUri, errorDetails);
        return controller.BadRequest();
    }
}
