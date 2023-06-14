using System.Diagnostics.CodeAnalysis;
using System.Web;

namespace Rinkudesu.Gateways.Utils;

public static class StringUtils
{
    [ExcludeFromCodeCoverage]
    [SuppressMessage("Design", "CA1054", MessageId = "URI-like parameters should not be strings")]
    public static Uri ToUri(this string uriString) => new Uri(uriString, UriKind.RelativeOrAbsolute);

    [ExcludeFromCodeCoverage]
    [SuppressMessage("Design", "CA1055:URI-like return values should not be strings")]
    public static string UriQueryEncode(this string queryString) => HttpUtility.UrlEncode(queryString);
}
