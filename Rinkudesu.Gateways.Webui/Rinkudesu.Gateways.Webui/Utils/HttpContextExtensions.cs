﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http;
using Rinkudesu.Gateways.Utils;
using Rinkudesu.Gateways.Webui.Controllers;

namespace Rinkudesu.Gateways.Webui.Utils
{
    public static class HttpContextExtensions
    {
        [ExcludeFromCodeCoverage]
        public static Uri GetCurrentUrl(this HttpContext context)
        {
            return $"{context.Request.Path}{context.Request.QueryString}".ToUri();
        }

        [ExcludeFromCodeCoverage]
        public static Uri GetBasePath(this HttpContext context)
        {
            return $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}/".ToUri();
        }

        [ExcludeFromCodeCoverage]
        public static string GetEncodedBasePath(this HttpContext context)
        {
            return UrlEncoder.Default.Encode(context.GetBasePath().ToString());
        }

        [ExcludeFromCodeCoverage]
        public static void AddErrorDetails(this HttpContext context, Uri redirectUri, string? errorDetails = null)
        {
            context.Items.Add(ErrorController.RETURN_URL_ITEM_NAME, redirectUri);
            if (!string.IsNullOrEmpty(errorDetails))
                context.Items.Add(ErrorController.ERROR_DETAILS_ITEM_NAME, errorDetails);
        }
    }
}
