using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;

namespace Rinkudesu.Gateways.Webui.Utils;

[ExcludeFromCodeCoverage]
public class RedisCacheTicketStore : ITicketStore
{
    private const string PREFIX = "SessionTicket-";

    private readonly IDistributedCache _cache;

    public RedisCacheTicketStore(IDistributedCache cache)
    {
        _cache = cache;
    }

    public RedisCacheTicketStore(RedisCacheOptions options)
    {
        _cache = new RedisCache(options);
    }

    public async Task<string> StoreAsync(AuthenticationTicket ticket)
    {
        var random = RandomNumberGenerator.GetBytes(512);
        var key = PREFIX + Convert.ToBase64String(random);
        await SetTicket(key, ticket);
        return key;
    }

    public async Task RenewAsync(string key, AuthenticationTicket ticket)
    {
        await SetTicket(key, ticket);
    }

    public async Task<AuthenticationTicket?> RetrieveAsync(string key)
    {
        var bytes = await _cache.GetAsync(key);
        if (bytes is null)
            return null;
        return TicketSerializer.Default.Deserialize(bytes);
    }

    public async Task RemoveAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }

    private async Task SetTicket(string id, AuthenticationTicket ticket)
    {
        var cacheOptions = new DistributedCacheEntryOptions();
        var ticketBytes = TicketSerializer.Default.Serialize(ticket);
        if (ticket.Properties.ExpiresUtc is {} expiration)
            cacheOptions.AbsoluteExpiration = expiration;
        await _cache.SetAsync(id, ticketBytes, cacheOptions);
    }
}
