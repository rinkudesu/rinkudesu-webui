using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Rinkudesu.Gateways.Clients.Identity;

namespace Rinkudesu.Gateways.Clients;

public abstract class AccessTokenClient : MicroserviceClient
{
    private readonly IdentityClient _identityClient;

    protected AccessTokenClient(HttpClient client, ILogger<AccessTokenClient> logger, IdentityClient identityClient) : base(client, logger)
    {
        _identityClient = identityClient;
    }

    public async Task SetAccessToken(HttpRequest controllerRequest)
    {
        var token = await _identityClient.ReadIdentityCookie(controllerRequest).RequestJwt().ConfigureAwait(false);
        Client.DefaultRequestHeaders.Authorization = new ("Bearer", token);
    }
}
