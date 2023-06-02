using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Rinkudesu.Gateways.Webui.Controllers.AutocompletionApi;

[ApiController]
[Route("api/autocompletion/[controller]")]
[Authorize]
[ExcludeFromCodeCoverage]
public partial class LinkTitleAutocompletion : ControllerBase
{
    private readonly HttpClient _client;

    public LinkTitleAutocompletion(HttpClient client)
    {
        _client = client;
    }

    // This is here because CORS exists and there were issues getting this title directly from the user's browser.
    // This is not perfect and should probably be expunged from here, but I don't really see a better way of doing this ATM.
    // todo: this doesn't fit here and should be solved in a better way
    [HttpGet]
    public async Task<ActionResult> GetTitleFromUrl([FromQuery] Uri url, CancellationToken cancellationToken)
    {
        // invoke a hard timeout limit for the entire request so as to avoid potential DOS attempts
        using var timeoutToken = new CancellationTokenSource(TimeSpan.FromSeconds(3));
        using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(timeoutToken.Token, cancellationToken);
        using var response = await _client.GetAsync(url, linkedToken.Token);

        if (response.Content.Headers.ContentType!.MediaType != "text/html")
            return NotFound();
        var html = await response.Content.ReadAsStringAsync(linkedToken.Token);
        var title = TitleRegex().Match(html); //thanks: https://stackoverflow.com/a/329324
        var match = title.Groups["Title"].Value;
        if (string.IsNullOrWhiteSpace(match))
            return NotFound();
        return Ok(match);
    }

    [GeneratedRegex("\\<title\\b[^>]*\\>\\s*(?<Title>[\\s\\S]*?)\\</title\\>")]
    private static partial Regex TitleRegex();
}
