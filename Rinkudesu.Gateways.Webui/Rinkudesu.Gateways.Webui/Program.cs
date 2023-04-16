using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using CommandLine;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Rinkudesu.Gateways.Webui;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;

#pragma warning disable CA1812
#pragma warning disable CA1305
#pragma warning disable CA1852

var result = Parser.Default.ParseArguments<InputArguments>(args)
    .WithParsed(o => {
        o.SaveAsCurrent();
    });
if (result.Tag == ParserResultType.NotParsed)
{
    return 1;
}
var logConfig = new LoggerConfiguration();
if (!InputArguments.Current.MuteConsoleLog)
{
    logConfig.WriteTo.Console();
}
Log.Logger = logConfig.CreateBootstrapLogger();

try
{
    var webApp = WebApplication.CreateBuilder(args);

    var startup = new Startup(webApp.Configuration);
    startup.ConfigureServices(webApp.Services);

    webApp.Host.UseSerilog((context, services, configuration) => {
        if (!InputArguments.Current.MuteConsoleLog)
        {
            configuration.WriteTo.Console();
        }
        if (InputArguments.Current.FileLogPath != null)
        {
            configuration.WriteTo.File(new RenderedCompactJsonFormatter(),
                InputArguments.Current.FileLogPath);
        }
        InputArguments.Current.GetMinimumLogLevel(configuration);
        configuration.ReadFrom.Services(services);
        configuration.Enrich.FromLogContext()
            .Enrich.WithProperty("ApplicationName", context.HostingEnvironment.ApplicationName)
            .Enrich.WithExceptionDetails();
    });

    // This is useful for selenium tests, that sometimes might require https, even if invalid
    if (File.Exists("./cert.pfx"))
    {
        webApp.WebHost.UseKestrel(o => {
            o.ConfigureHttpsDefaults(h => {
                h.ServerCertificate = new X509Certificate2("./cert.pfx");
            });
        });
    }

    var app = webApp.Build();
    startup.Configure(app, app.Environment);

    await app.RunAsync();

}
#pragma warning disable CA1031
catch (Exception e)
#pragma warning restore CA1031
{
    Log.Fatal(e, "Application failed to start");
    return 2;
}
finally
{
    Log.CloseAndFlush();
}
return 0;
