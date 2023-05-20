using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rinkudesu.Gateways.Clients;

namespace Rinkudesu.Gateways.Webui.Controllers;

public abstract class AccessTokenClientController<TClient> : Controller where TClient : AccessTokenClient
{
    private readonly TClient _client;
    private bool tokenSet;

    protected TClient Client
    {
        get
        {
            if (!tokenSet)
                throw new InvalidOperationException("JWT must be set first");

            return _client;
        }
    }

    protected AccessTokenClientController(TClient client)
    {
        _client = client;
    }

    protected async Task SetJwt()
    {
        if (tokenSet)
            return;

        await _client.SetAccessToken(HttpContext.Request);
        tokenSet = true;
    }
}
