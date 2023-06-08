using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Rinkudesu.Gateways.Clients;

public abstract class MicroserviceClient
{
    protected ILogger<MicroserviceClient> Logger { get; }
    protected HttpClient Client { get; }

    /// <summary>
    /// Reads text returned from a microservice if the response code was not 2XX.
    /// </summary>
    public string? LastErrorReturned { get; private set; }

    protected MicroserviceClient(HttpClient client, ILogger<MicroserviceClient> logger)
    {
        Logger = logger;
        Client = client;
    }

    protected async Task<TDto?> HandleMessageAndParseDto<TDto>(HttpResponseMessage message, string logId, CancellationToken token) where TDto : class
    {
        if (!message.IsSuccessStatusCode)
        {
            await SetLastErrorIfAny(message, token).ConfigureAwait(false);

            if (message.StatusCode == HttpStatusCode.NotFound)
            {
                Logger.LogInformation("Object in client {ClientName} with {LogId} not found", GetType().Name, logId);
            }
            else
            {
                Logger.LogWarning("Unexpected response code while querying for object in client {ClientName} with {LogId}: '{StatusCode}'", GetType().Name, logId, message.StatusCode);
            }
            return null;
        }

        try
        {
            var stream = await message.Content.ReadAsStreamAsync(token).ConfigureAwait(false);
            return await JsonSerializer.DeserializeAsync<TDto>(stream, CommonSettings.JsonOptions, token).ConfigureAwait(false);
        }
        catch (JsonException e)
        {
            Logger.LogWarning(e, "Unable to parse object in client {ClientType} with id {LogId}", GetType().Name, logId);
            return null;
        }
    }

    protected static StringContent GetJsonContent<TDto>(TDto dto)
    {
        var message = JsonSerializer.Serialize(dto, CommonSettings.JsonOptions);
        return new StringContent(message, Encoding.UTF8, "application/json");
    }

    protected async Task SetLastErrorIfAny(HttpResponseMessage responseMessage, CancellationToken token)
    {
        var response = await responseMessage.Content.ReadAsStringAsync(cancellationToken: token).ConfigureAwait(false);
        if (!string.IsNullOrEmpty(response))
            LastErrorReturned = response;
    }
}
