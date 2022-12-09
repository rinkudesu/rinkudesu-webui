using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rinkudesu.Gateways.Utils;

namespace Rinkudesu.Gateways.Clients.Links
{
    public class LinksClient : AccessTokenClient
    {
        public LinksClient(HttpClient client, ILogger<LinksClient> logger) : base(client, logger)
        {
        }

        public async Task<LinkDto?> GetLink(Guid id, CancellationToken token = default)
        {
            try
            {
                using var message = await Client.GetAsync($"links/{id}".ToUri(), token).ConfigureAwait(false);
                return await HandleMessageAndParseDto<LinkDto>(message, $"id {id}", token).ConfigureAwait(false);
            }
            catch (HttpRequestException e)
            {
                Logger.LogWarning(e, "Error while requesting link with id '{id}'", id);
                return null;
            }
        }

        public async Task<LinkDto?> GetLink(string key, CancellationToken token = default)
        {
            try
            {
                using var message = await Client.GetAsync($"links/{key}".ToUri(), token).ConfigureAwait(false);
                return await HandleMessageAndParseDto<LinkDto>(message, "shareable key", token).ConfigureAwait(false);
            }
            catch (HttpRequestException e)
            {
                Logger.LogWarning(e, "Error while requesting link with shareable key");
                return null;
            }
        }

        public async Task<bool> CreateLink(LinkDto newLink, CancellationToken token = default)
        {
            try
            {
                using var content = GetJsonContent(newLink);
                var response = await Client.PostAsync("links".ToUri(), content, token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode) return true;
                Logger.LogWarning($"Unable to create new link. Response code was '{response.StatusCode}'.");
                return false;
            }
            catch (JsonException e)
            {
                Logger.LogWarning(e, "Unable to serialise new link into json");
                return false;
            }
            catch (HttpRequestException e)
            {
                Logger.LogWarning(e, "Error while requesting new link creation.");
                return false;
            }
        }

        public async Task<IEnumerable<LinkDto>?> GetLinks(CancellationToken token = default)
        {
            try
            {
                var response = await Client.GetAsync("links?showPrivate=true".ToUri(), token).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    Logger.LogWarning("Received non-success status code '{statusCode}' from links microservice", response.StatusCode);
                    return null;
                }
                return await HandleMessageAndParseDto<IEnumerable<LinkDto>>(response, "all links", token).ConfigureAwait(false);
            }
            catch (JsonException e)
            {
                Logger.LogWarning(e, "Unable to parse links");
                return null;
            }
            catch (HttpRequestException e)
            {
                Logger.LogWarning(e, "Unable to receive links from microservice");
                return null;
            }
        }

        public async Task<bool> Delete(Guid id, CancellationToken token = default)
        {
            try
            {
                var response = await Client.DeleteAsync($"links/{id}".ToUri(), token).ConfigureAwait(false);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException e)
            {
                Logger.LogWarning(e, "Unable to delete link with id '{id}'", id);
                return false;
            }
        }

        public async Task<bool> Edit(Guid id, LinkDto link, CancellationToken token = default)
        {
            try
            {
                using var content = GetJsonContent(link);
                var response = await Client.PostAsync($"links/{id}".ToUri(), content, token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode) return true;

                Logger.LogWarning($"Unable to edit link with id {id}. Response code was '{response.StatusCode}'.");
                return false;
            }
            catch (JsonException e)
            {
                Logger.LogWarning(e, "Unable to serialise link into json");
                return false;
            }
            catch (HttpRequestException e)
            {
                Logger.LogWarning(e, "Error while requesting new link creation.");
                return false;
            }
        }
    }
}
