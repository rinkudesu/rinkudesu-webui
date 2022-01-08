using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Rinkudesu.Gateways.Clients.Exceptions;
using Rinkudesu.Gateways.Utils;

namespace Rinkudesu.Gateways.Clients.Links;

public class SharedLinksClient : IAuthorisedMicroserviceClient<SharedLinksClient>
{
    private readonly HttpClient _client;

    public SharedLinksClient(HttpClient client)
    {
        _client = client;
    }

    public SharedLinksClient SetAccessToken(string token)
    {
        _client.DefaultRequestHeaders.Authorization = new("Bearer", token);
        return this;
    }

    public async Task<bool> IsShared(Guid id, CancellationToken cancellationToken = default)
    {
        using var response = await _client.GetAsync($"shares/{id}".ToUri(), cancellationToken).ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }
        if (response.StatusCode == HttpStatusCode.OK)
        {
            return true;
        }
        throw new UnexpectedResponseException(response.StatusCode, nameof(SharedLinksClient));
    }

    public async Task<string> GetKey(Guid id, CancellationToken cancellationToken = default)
    {
        return await _client.GetStringAsync($"shares/{id}".ToUri(), cancellationToken).ConfigureAwait(false);
    }

    public async Task<string> Share(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _client.PostAsync($"shares/{id}".ToUri(), null, cancellationToken).ConfigureAwait(false);

        if (response.StatusCode != HttpStatusCode.Created)
        {
            throw new UnexpectedResponseException(response.StatusCode, nameof(SharedLinksClient));
        }
        return await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task Unshare(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _client.DeleteAsync($"shares/{id}".ToUri(), cancellationToken).ConfigureAwait(false);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new UnexpectedResponseException(response.StatusCode, nameof(SharedLinksClient));
        }
    }
}