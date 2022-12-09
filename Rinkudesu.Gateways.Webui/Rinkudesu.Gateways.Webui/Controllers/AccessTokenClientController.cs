﻿using Microsoft.AspNetCore.Mvc;
using Rinkudesu.Gateways.Clients;
using Rinkudesu.Gateways.Webui.Utils;

namespace Rinkudesu.Gateways.Webui.Controllers;

public abstract class AccessTokenClientController<TClient> : Controller where TClient : AccessTokenClient
{
    private readonly TClient _client;
    private bool tokenSet;

    protected TClient Client
    {
        get
        {
            if (tokenSet)
                return _client;

            _client.SetAccessToken(HttpContext.GetJwt());
            tokenSet = true;
            return _client;
        }
    }

    protected AccessTokenClientController(TClient client)
    {
        _client = client;
    }
}