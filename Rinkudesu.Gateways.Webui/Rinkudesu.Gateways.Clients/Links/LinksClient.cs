using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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

        public async Task<bool> CreateLink(LinkDto newLink, CancellationToken token = default)
        {
            string message;
            try
            {
                message = JsonSerializer.Serialize(newLink, jsonOptions);
            }
            catch (JsonException e)
            {
                _logger.LogWarning(e, "Unable to serialise new link into json");
                return false;
            }
            try
            {
                var content = new StringContent(message, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("links", content, token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode) return true;
                _logger.LogWarning($"Unable to create new link. Response code was '{response.StatusCode}'.");
                return false;
            }
            catch (HttpRequestException e)
            {
                _logger.LogWarning(e, "Error while requesting new link creation.");
                return false;
            }
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

        public async Task<bool> Delete(Guid id, CancellationToken token = default)
        {
            try
            {
                var response = await _client.DeleteAsync($"links/{id}", token).ConfigureAwait(false);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException e)
            {
                _logger.LogWarning(e, "Unable to delete link with id '{id}'", id);
                return false;
            }
        }
    }
}