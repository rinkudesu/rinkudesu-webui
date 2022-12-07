using System.Net.Http;

namespace Rinkudesu.Gateways.Clients;

public abstract class AccessTokenClient
{
    protected HttpClient Client { get; }

    protected AccessTokenClient(HttpClient client)
    {
        Client = client;
    }

    public AccessTokenClient SetAccessToken(string token)
    {
        Client.DefaultRequestHeaders.Authorization = new ("Bearer", token);
        return this;
    }
}
