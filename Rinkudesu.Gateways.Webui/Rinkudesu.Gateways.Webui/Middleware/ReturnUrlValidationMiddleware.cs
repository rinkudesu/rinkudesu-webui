using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace Rinkudesu.Gateways.Webui.Middleware;

public class ReturnUrlValidationMiddleware
{
    private readonly RequestDelegate _next;

    public ReturnUrlValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    private const string RETURN_URL_NAME = "returnUrl";
#pragma warning disable SYSLIB1045
    private readonly Regex _returnUrlRegex = new("[rR]eturn[uU]rl[^=]*=[^?]*[&$]");
#pragma warning restore SYSLIB1045

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Query.TryGetValue(RETURN_URL_NAME, out var returnUrl))
        {
            var sanitised = SanitiseUrl(returnUrl!, encode: true);
            var newQueryString = _returnUrlRegex.Replace(context.Request.QueryString.Value!, $"returnUrl={sanitised}&");
            context.Request.QueryString = new QueryString(newQueryString);
        }
        if (context.Request.HasFormContentType && context.Request.Form.TryGetValue(RETURN_URL_NAME, out returnUrl))
        {
            var sanitised = SanitiseUrl(returnUrl!);
            var formDictionary = context.Request.Form.Where(f => f.Key != RETURN_URL_NAME).ToDictionary(k => k.Key, v => v.Value);
            formDictionary[RETURN_URL_NAME] = sanitised;
            context.Request.Form = new FormCollection(formDictionary, context.Request.Form.Files);
        }
        await _next(context);
    }

    private static string SanitiseUrl(string originalUrl, bool encode = false)
    {
        if (!Uri.TryCreate(originalUrl, UriKind.RelativeOrAbsolute, out var returnUri))
            return originalUrl; //let it be if it doesn't parse so that we don't replace valid url elements by mistake

        var sanitised = originalUrl;
        if (returnUri.IsAbsoluteUri)
            sanitised = returnUri.PathAndQuery;

        if (encode)
            sanitised = UrlEncoder.Default.Encode(sanitised);

        return sanitised;
    }
}
