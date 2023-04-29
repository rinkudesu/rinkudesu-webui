using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Caching.StackExchangeRedis;

namespace Rinkudesu.Gateways.Webui.Models;

[ExcludeFromCodeCoverage]
public class RedisSettings
{
    private static RedisSettings? current;

    public static RedisSettings Current
    {
        get => current ?? throw new InvalidOperationException("Redis settings have not been initialised");
        set
        {
            if (current is not null)
                throw new InvalidOperationException("Redis settings have already been initialised");

            current = value;
        }
    }

    public string Address { get; }

    public RedisSettings()
    {
        Address = Environment.GetEnvironmentVariable("RINKUDESU_REDIS_ADDRESS") ?? throw new InvalidOperationException("RINKUDESU_REDIS_ADDRESS env variable must be set");
    }

    [SuppressMessage("Design", "CA1024:Use properties where appropriate")] // this is most certainly not appropriate
    public RedisCacheOptions GetRedisOptions() => new RedisCacheOptions { Configuration = Address };
}
