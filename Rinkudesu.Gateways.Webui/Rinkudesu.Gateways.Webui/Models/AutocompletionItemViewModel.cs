using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Rinkudesu.Gateways.Webui.Models;

[ExcludeFromCodeCoverage]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class AutocompletionItemViewModel
{
    [JsonPropertyName("id")]
    public string ItemId { get; init; } = string.Empty;
    [JsonPropertyName("data")]
    public string ItemData { get; init; } = string.Empty;
}
