using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Rinkudesu.Gateways.Webui.Models;

[ExcludeFromCodeCoverage]
public class AutocompletionItemViewModel
{
    [JsonPropertyName("id")]
    public string ItemId { get; set; }
    [JsonPropertyName("data")]
    public string ItemData { get; set; }
}
