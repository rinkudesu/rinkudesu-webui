using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Rinkudesu.Gateways.Clients.Identity;

[ExcludeFromCodeCoverage]
public class AccountDeleteDto
{
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
}
