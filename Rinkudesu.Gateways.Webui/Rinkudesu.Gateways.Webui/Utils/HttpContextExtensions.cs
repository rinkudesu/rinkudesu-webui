using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http;
using Rinkudesu.Gateways.Utils;

namespace Rinkudesu.Gateways.Webui.Utils
{
    public static class HttpContextExtensions
    {
        public static Uri GetCurrentUrl(this HttpContext context)
        {
            return $"{context.Request.Path}{context.Request.QueryString}".ToUri();
        }

        public static Uri GetBasePath(this HttpContext context)
        {
            return $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}/".ToUri();
        }

        public static string GetEncodedBasePath(this HttpContext context)
        {
            return UrlEncoder.Default.Encode(context.GetBasePath().ToString());
        }
    }
}