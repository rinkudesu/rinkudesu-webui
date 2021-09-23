using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Rinkudesu.Gateways.Clients.Links
{
    public class LinksClient
    {
        private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
            { PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter() }};

        private readonly HttpClient _client;
        private readonly ILogger<LinksClient> _logger;

        public LinksClient(HttpClient client, ILogger<LinksClient> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<IEnumerable<LinkDto>?> GetLinks(CancellationToken token = default)
        {
            try
            {
                var response = await _client.GetAsync("links", token).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Received non-success status code '{statusCode}' from links microservice", response.StatusCode);
                    return null;
                }
                try
                {
                    var links = await JsonSerializer.DeserializeAsync<IEnumerable<LinkDto>>(
                        await response.Content.ReadAsStreamAsync().ConfigureAwait(false), jsonOptions, token).ConfigureAwait(false);
                    return links;
                }
                catch (JsonException e)
                {
                    _logger.LogWarning(e, "Unable to parse links");
                    return null;
                }
            }
            catch (HttpRequestException e)
            {
                _logger.LogWarning(e, "Unable to receive links from microservice");
                return null;
            }
        }
    }
}