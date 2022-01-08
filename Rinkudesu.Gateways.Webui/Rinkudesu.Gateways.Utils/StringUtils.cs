using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Rinkudesu.Gateways.Utils;

public static class StringUtils
{
    [ExcludeFromCodeCoverage]
    [SuppressMessage("Design", "CA1054", MessageId = "URI-like parameters should not be strings")]
    public static Uri ToUri(this string uriString) => new Uri(uriString, UriKind.RelativeOrAbsolute);

    public static string ToBase64(this string text)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
    }
}