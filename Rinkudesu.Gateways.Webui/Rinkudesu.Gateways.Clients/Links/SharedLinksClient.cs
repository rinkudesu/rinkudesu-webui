using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rinkudesu.Gateways.Clients.Exceptions;
using Rinkudesu.Gateways.Clients.Identity;
using Rinkudesu.Gateways.Utils;

namespace Rinkudesu.Gateways.Clients.Links;

public class SharedLinksClient : AccessTokenClient
{
    public SharedLinksClient(HttpClient client, ILogger<SharedLinksClient> logger, IdentityClient identityClient) : base(client, logger, identityClient)
    {
    }

    public async Task<bool> IsShared(Guid id, CancellationToken cancellationToken = default)
    {
        using var response = await Client.GetAsync($"shares/{id}".ToUri(), cancellationToken).ConfigureAwait(false);

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
        return await Client.GetStringAsync($"shares/{id}".ToUri(), cancellationToken).ConfigureAwait(false);
    }

    public async Task<string> Share(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await Client.PostAsync($"shares/{id}".ToUri(), null, cancellationToken).ConfigureAwait(false);

        if (response.StatusCode != HttpStatusCode.Created)
        {
            throw new UnexpectedResponseException(response.StatusCode, nameof(SharedLinksClient));
        }
        return await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task Unshare(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await Client.DeleteAsync($"shares/{id}".ToUri(), cancellationToken).ConfigureAwait(false);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new UnexpectedResponseException(response.StatusCode, nameof(SharedLinksClient));
        }
    }
}
