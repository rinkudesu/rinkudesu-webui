using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rinkudesu.Gateways.Clients;

[ExcludeFromCodeCoverage]
public static class CommonSettings
{
    public static readonly JsonSerializerOptions JsonOptions = new()
        { PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter() } };
}
