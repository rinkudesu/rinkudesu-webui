using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Rinkudesu.Gateways.Webui.Middleware;

[ExcludeFromCodeCoverage]
public class AccessTokenReaderMiddleware
{
    private readonly RequestDelegate _next;

    public AccessTokenReaderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Items.ContainsKey("JwtToken"))
        {
            var token = await context.GetTokenAsync("access_token");
            context.Items.Add("JwtToken", token ?? string.Empty);
        }
        await _next(context);
    }
}
