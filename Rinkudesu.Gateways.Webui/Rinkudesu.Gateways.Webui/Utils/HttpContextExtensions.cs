using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http;

namespace Rinkudesu.Gateways.Webui.Utils
{
    public static class HttpContextExtensions
    {
        public static string GetCurrentUrl(this HttpContext context)
        {
            return $"{context.Request.Path}{context.Request.QueryString}";
        }

        public static string GetBasePath(this HttpContext context)
        {
            return $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}/";
        }

        public static string GetEncodedBasePath(this HttpContext context)
        {
            return UrlEncoder.Default.Encode(context.GetBasePath());
        }
    }
}