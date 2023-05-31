using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rinkudesu.Gateways.Clients.Identity;
using Rinkudesu.Gateways.MessageQueues;
using Rinkudesu.Gateways.Utils;
using Rinkudesu.Gateways.Webui.Models;
using Rinkudesu.Gateways.Webui.Utils;
using Rinkudesu.Kafka.Dotnet.Base;

namespace Rinkudesu.Gateways.Webui.Controllers;

public class AccountCreationController : Controller
{
    private readonly IdentityClient _client;
    private readonly IKafkaProducer _kafkaProducer;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<AccountCreationController> _localizer;

    public AccountCreationController(IdentityClient client, IKafkaProducer kafkaProducer, IMapper mapper, IStringLocalizer<AccountCreationController> localizer)
    {
        _client = client;
        _kafkaProducer = kafkaProducer;
        _mapper = mapper;
        _localizer = localizer;
    }

    [HttpGet]
    public IActionResult Index() => View(new RegisterAccountViewModel());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAccount([Bind] RegisterAccountViewModel model)
    {
        if (!EnvironmentalVariableReader.RegistrationEnabled)
            return this.ReturnBadRequest("/".ToUri(), "Registration disabled");

        if (!ModelState.IsValid || model.PasswordMismatch)
            return this.ReturnBadRequest(Url.ActionLink(nameof(Index))!.ToUri());

        var result = await _client.RegisterAccount(_mapper.Map<RegisterAccountDto>(model));
        if (result is null)
        {
            if (_client.LastErrorReturned == "Email already exists")
                await _kafkaProducer.ProduceSendEmail(Guid.Empty, _localizer["welcome"], _localizer["emailUsed"], true, forceAnotherEmail: model.Email);
            else
                return this.ReturnBadRequest(Url.ActionLink(nameof(Index))!.ToUri());
        }
        else
            await _kafkaProducer.ProduceSendEmail(result.UserId, _localizer["welcome"], _localizer["greetings"] + $"<a href='{HttpContext.GetBasePath()}{Url.Action(nameof(ConfirmEmail), new { result.UserId, result.EmailConfirmationToken })!.TrimStart('/')}'>{_localizer["click"]}</a>", true);

        return View("AccountCreated");
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(Guid userId, string emailConfirmationToken)
    {
        var result = await _client.ConfirmEmail(new ConfirmEmailDto
        {
            EmailToken = emailConfirmationToken,
            UserId = userId,
        });
        if (result)
            return LocalRedirect("/");
        return this.ReturnBadRequest("/".ToUri());
    }
}
