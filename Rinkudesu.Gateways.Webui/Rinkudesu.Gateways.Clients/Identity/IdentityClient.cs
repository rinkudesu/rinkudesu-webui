﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Rinkudesu.Gateways.Utils;

namespace Rinkudesu.Gateways.Clients.Identity;

public class IdentityClient : MicroserviceClient
{
    private string? identityCookie;
    //todo: figure out how to inject that during service resolution as it gets pretty weird when using "AddHttpClient".
    public string? IdentityCookieName { get; set; } = ".rinkudesu.session";

    public IdentityClient(HttpClient client, ILogger<MicroserviceClient> logger) : base(client, logger)
    {
    }

    public IdentityClient ReadIdentityCookie(HttpRequest request)
    {
        if (string.IsNullOrWhiteSpace(IdentityCookieName))
            throw new InvalidOperationException("Identity cookie name must be set first");

        identityCookie = request.Cookies[IdentityCookieName];
        if (StringValues.IsNullOrEmpty(identityCookie))
            throw new ArgumentException(@"This request does not contain identity cookies", nameof(request));

        return this;
    }

    public async Task<bool> PostLogin(HttpResponse controllerResponse, string userName, string password)
    {
        var formDataValues = new Dictionary<string, string>
        {
            { "userName", userName },
            { "password", password },
        };
        using var formData = new FormUrlEncodedContent(formDataValues);
        using var response = await Client.PostAsync("session/login".ToUri(), formData).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            Logger.LogWarning("Failed to authenticate user {UserName}: identity service returned status code {StatusCode}", userName, response.StatusCode.ToString());
            return false;
        }
        var identityCookies = response.Headers.GetValues("Set-Cookie").ToList();
        if (identityCookies.Count == 0)
        {
            Logger.LogWarning("Failed to authenticate user {UserName}: identity cookies are missing from session service response", userName);
            return false;
        }
        foreach (var identityCookie in identityCookies)
        {
            controllerResponse.Headers["Set-Cookie"] = StringValues.Concat(controllerResponse.Headers["Set-Cookie"], identityCookie);
        }
        return true;
    }

    public async Task<bool> PostLogOut()
    {
        EnsureIdentityCookieSet();
        using var request = new HttpRequestMessage(HttpMethod.Post, "session/logout".ToUri());
        AppendIdentityToRequest(request);
        using var response = await Client.SendAsync(request).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
            return true;
        Logger.LogWarning("Unexpected status code when logging out user: {StatusCode}", response.StatusCode.ToString());
        return false;
    }

    public async Task<string?> RequestJwt()
    {
        EnsureIdentityCookieSet();
        using var request = new HttpRequestMessage(HttpMethod.Get, "Jwt".ToUri());
        AppendIdentityToRequest(request);
        using var response = await Client.SendAsync(request).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            Logger.LogWarning("Failed to get jwt: {StatusCode}", response.StatusCode.ToString());
            return null;
        }
        return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
    }

    private void EnsureIdentityCookieSet()
    {
        if (string.IsNullOrWhiteSpace(identityCookie))
            throw new InvalidOperationException("You must set the identity cookie value first");
    }

    private void AppendIdentityToRequest(HttpRequestMessage request) => request.Headers.Add("Cookie", $"{IdentityCookieName}={identityCookie};");
}