using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rinkudesu.Gateways.Utils;

namespace Rinkudesu.Gateways.Clients.Links
{
    public class LinksClient : IAuthorisedMicroserviceClient<LinksClient>
    {
        private static JsonSerializerOptions JsonOptions => CommonSettings.JsonOptions;

        private readonly HttpClient _client;
        private readonly ILogger<LinksClient> _logger;

        public LinksClient(HttpClient client, ILogger<LinksClient> logger)
        {
            _client = client;
            _logger = logger;
        }

        public LinksClient SetAccessToken(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new ("Bearer", token);
            return this;
        }

        public async Task<LinkDto?> GetLink(Guid id, CancellationToken token = default)
        {
            try
            {
                using var message = await _client.GetAsync($"links/{id}".ToUri(), token).ConfigureAwait(false);
                return await HandleMessageAndParseLink(message, $"id {id}", token).ConfigureAwait(false);
            }
            catch (HttpRequestException e)
            {
                _logger.LogWarning(e, "Error while requesting link with id '{id}'", id);
                return null;
            }
        }

        public async Task<LinkDto?> GetLink(string key, CancellationToken token = default)
        {
            try
            {
                using var message = await _client.GetAsync($"links/{key.ToBase64()}".ToUri(), token).ConfigureAwait(false);
                return await HandleMessageAndParseLink(message, "shareable key", token).ConfigureAwait(false);
            }
            catch (HttpRequestException e)
            {
                _logger.LogWarning(e, "Error while requesting link with shareable key");
                return null;
            }
        }

        private async Task<LinkDto?> HandleMessageAndParseLink(HttpResponseMessage message, string linkLogId, CancellationToken token)
        {
            if (!message.IsSuccessStatusCode)
            {
                if (message.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogInformation($"Link with {linkLogId} not found");
                }
                else
                {
                    _logger.LogWarning($"Unexpected response code while querying for link with {linkLogId}: '{message.StatusCode}'");
                }
                return null;
            }

            try
            {
                var stream = await message.Content.ReadAsStreamAsync(token).ConfigureAwait(false);
                return await JsonSerializer.DeserializeAsync<LinkDto>(stream, JsonOptions, token).ConfigureAwait(false);
            }
            catch (JsonException e)
            {
                _logger.LogWarning(e, $"Unable to parse link with {linkLogId}");
                return null;
            }
        }

        public async Task<bool> CreateLink(LinkDto newLink, CancellationToken token = default)
        {
            string message;
            try
            {
                message = JsonSerializer.Serialize(newLink, JsonOptions);
            }
            catch (JsonException e)
            {
                _logger.LogWarning(e, "Unable to serialise new link into json");
                return false;
            }
            try
            {
                using var content = new StringContent(message, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("links".ToUri(), content, token).ConfigureAwait(false);
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
                var response = await _client.GetAsync("links?showPrivate=true".ToUri(), token).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Received non-success status code '{statusCode}' from links microservice", response.StatusCode);
                    return null;
                }
                try
                {
                    var links = await JsonSerializer.DeserializeAsync<IEnumerable<LinkDto>>(
                        await response.Content.ReadAsStreamAsync(token).ConfigureAwait(false), JsonOptions, token).ConfigureAwait(false);
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
                var response = await _client.DeleteAsync($"links/{id}".ToUri(), token).ConfigureAwait(false);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException e)
            {
                _logger.LogWarning(e, "Unable to delete link with id '{id}'", id);
                return false;
            }
        }

        public async Task<bool> Edit(Guid id, LinkDto link, CancellationToken token = default)
        {
            string json;
            try
            {
                json = JsonSerializer.Serialize(link, JsonOptions);
            }
            catch (JsonException e)
            {
                _logger.LogWarning(e, "Unable to serialise link into json");
                return false;
            }

            try
            {
                using var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync($"links/{id}".ToUri(), content, token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode) return true;

                _logger.LogWarning($"Unable to edit link with id {id}. Response code was '{response.StatusCode}'.");
                return false;
            }
            catch (HttpRequestException e)
            {
                _logger.LogWarning(e, "Error while requesting new link creation.");
                return false;
            }
        }
    }
}