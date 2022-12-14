using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rinkudesu.Gateways.Clients.Tags;
using Rinkudesu.Gateways.Utils;

namespace Rinkudesu.Gateways.Clients.LinkTags;

public class LinkTagsClient : AccessTokenClient
{
    public LinkTagsClient(HttpClient client, ILogger<LinkTagsClient> logger) : base(client, logger)
    {
    }

    public async Task<IEnumerable<TagDto>?> GetTagsForLink(Guid linkId, CancellationToken cancellationToken = default)
    {
        try
        {
            using var message = await Client.GetAsync($"linkTags/getTagsForLink/{linkId.ToString()}".ToUri(), cancellationToken).ConfigureAwait(false);
            return await HandleMessageAndParseDto<IEnumerable<TagDto>>(message, linkId.ToString(), cancellationToken).ConfigureAwait(false);
        }
        catch (JsonException e)
        {
            Logger.LogWarning(e, "Unable to serialise tags into json");
            return null;
        }
        catch (HttpRequestException e)
        {
            Logger.LogWarning(e, "Error while requesting tags for link.");
            return null;
        }
    }

    public async Task<IEnumerable<LinkTagDto>?> GetLinksForTag(Guid tagId, CancellationToken cancellationToken = default)
    {
        try
        {
            using var message = await Client.GetAsync($"linkTags/getLinksForTag/{tagId.ToString()}".ToUri(), cancellationToken).ConfigureAwait(false);
            return await HandleMessageAndParseDto<IEnumerable<LinkTagDto>>(message, tagId.ToString(), cancellationToken).ConfigureAwait(false);
        }
        catch (JsonException e)
        {
            Logger.LogWarning(e, "Unable to serialise LinkTags into json");
            return null;
        }
        catch (HttpRequestException e)
        {
            Logger.LogWarning(e, "Error while requesting links for tag.");
            return null;
        }
    }

    public async Task<bool> Assign(LinkTagDto newAssignment, CancellationToken cancellationToken = default)
    {
        try
        {
            using var content = GetJsonContent(newAssignment);
            var response = await Client.PostAsync("linkTags".ToUri(), content, cancellationToken).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                return true;

            Logger.LogWarning("Unable to create new link-tag assignment. Response code was {ResponseCode}", response.StatusCode);
            return false;
        }
        catch (JsonException e)
        {
            Logger.LogWarning(e, "Unable to serialise new link-tag assignment into json");
            return false;
        }
        catch (HttpRequestException e)
        {
            Logger.LogWarning(e, "Error while requesting new link-tag assignment creation.");
            return false;
        }
    }

    public async Task<bool> Delete(Guid linkId, Guid tagId, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await Client.DeleteAsync($"linkTags?linkId={linkId}&tagId={tagId}".ToUri(), cancellationToken).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                return true;

            Logger.LogWarning("Unable to remove link-tag assignment. Response code was {ResponseCode}", response.StatusCode);
            return false;
        }
        catch (HttpRequestException e)
        {
            Logger.LogWarning(e, "Error while deleting link-tag assignment.");
            return false;
        }
    }
}
