using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Rinkudesu.Gateways.Webui.Middleware;

[ExcludeFromCodeCoverage]
public class CustomErrorsHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public CustomErrorsHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    private readonly ImmutableHashSet<int> _supportedErrorCodes = new[] { 400, 404 }.ToImmutableHashSet();

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (!context.Response.HasStarted && _supportedErrorCodes.Contains(context.Response.StatusCode))
        {
            //null endpoint so that valid paths with invalid IDs will also work here
            context.SetEndpoint(null);
            context.Items["originalPath"] = context.Request.Path.Value;
            context.Request.Path = $"/error/error{context.Response.StatusCode.ToString(CultureInfo.InvariantCulture)}";
            //re-execute the request so that the user gets the correct page without redirects
            await _next(context);
        }
    }
}
