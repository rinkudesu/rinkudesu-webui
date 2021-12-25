using Microsoft.AspNetCore.Builder;
using Rinkudesu.Gateways.Webui;
#pragma warning disable CA1812

var webApp = WebApplication.CreateBuilder(args);

var startup = new Startup(webApp.Configuration);
startup.ConfigureServices(webApp.Services);

var app = webApp.Build();
startup.Configure(app, app.Environment);

await app.RunAsync();
