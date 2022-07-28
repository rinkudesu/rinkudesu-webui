using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rinkudesu.Gateways.Utils;

namespace Rinkudesu.Gateways.Clients.Tags;

public class TagsClient : IAuthorisedMicroserviceClient<TagsClient>
{
    private static JsonSerializerOptions JsonOptions => CommonSettings.JsonOptions;

    private readonly HttpClient _client;
    private readonly ILogger<TagsClient> _logger;

    public TagsClient(HttpClient client, ILogger<TagsClient> logger)
    {
        _client = client;
        _logger = logger;
    }

    public TagsClient SetAccessToken(string token)
    {
        _client.DefaultRequestHeaders.Authorization = new ("Bearer", token);
        return this;
    }

    public async Task ThisIsATestPleaseIgnore()
    {
        var response = await _client.GetAsync("tags".ToUri()).ConfigureAwait(false);
    }
}
