using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rinkudesu.Gateways.Clients.Tags;
using Rinkudesu.Gateways.Webui.Models;

namespace Rinkudesu.Gateways.Webui.Controllers.AutocompletionApi;

[ApiController]
[Route("api/autocompletion/[controller]")]
[Authorize]
[ExcludeFromCodeCoverage]
public class TagsAutocompletionApiController : AccessTokenClientControllerBase<TagsClient>
{
    public TagsAutocompletionApiController(TagsClient client) : base(client)
    {
    }

    public async Task<ActionResult> GetTags([FromQuery] string name, CancellationToken cancellationToken)
    {
        var query = new TagQueryDto { NameQuery = name };
        var results = await Client.GetTags(query, cancellationToken);
        if (results is null)
            return NotFound();

        return Ok(results.Select(r => new AutocompletionItemViewModel { ItemId = r.Id.ToString(), ItemData = r.Name }).ToArray());
    }
}
