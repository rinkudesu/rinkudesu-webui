using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Polly;
using Polly.Extensions.Http;
using Rinkudesu.Gateways.Clients.Links;
using Rinkudesu.Gateways.Clients.LinkTags;
using Rinkudesu.Gateways.Clients.Tags;
using Rinkudesu.Gateways.Utils;
using Rinkudesu.Gateways.Webui.Middleware;
using Rinkudesu.Gateways.Webui.Models;
using Rinkudesu.Kafka.Dotnet;
using Rinkudesu.Kafka.Dotnet.Base;

namespace Rinkudesu.Gateways.Webui
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
#if DEBUG
            IdentityModelEventSource.ShowPII = true;
#endif
            services.AddControllersWithViews().AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
#if DEBUG
                .AddRazorRuntimeCompilation()
#endif
                ;

            services.AddAutoMapper(typeof(MappingProfiles));

            SetupClients(services);
            SetupKafka(services);

            KeycloakSettings.Current = new KeycloakSettings();

#if DEBUG
            IdentityModelEventSource.ShowPII = true;
#endif

            services.AddAuthentication(options => {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => {
                    //forcefully revalidate cookie once jwt token is expired
                    //
                    // This is necessary as aspnet doesn't do this by itself for some reason when used together with a cookie.
                    // That would lead to expired JWTs being passed to other microservices, that would (correctly) reject them.
                    // What the line below does is it manually checks for the expiration date, and if it's passed, forces cookie revalidation, which in turn runs the challenge, thus refreshing the token.
                    options.Events.OnValidatePrincipal = context => {
                        if (context.Properties.Items.TryGetValue(".Token.expires_at", out var expireString))
                        {
                            var expiration = DateTime.Parse(expireString!, CultureInfo.InvariantCulture);
                            if (expiration < DateTime.Now)
                            {
                                context.ShouldRenew = true;
                                context.RejectPrincipal();
                            }
                        }
                        return Task.CompletedTask;
                    };
                })
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options => {
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.Authority = KeycloakSettings.Current.Authority;
                    options.ClientId = KeycloakSettings.Current.ClientId;
                    options.ResponseType = "code";
                    options.SaveTokens = true;
                    options.UseTokenLifetime = true;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.RequireHttpsMetadata = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("RINKUDESU_AUTHORITY_ALLOW_HTTP"));
                    options.TokenValidationParameters.NameClaimType = "preferred_username";
                    options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
                    options.TokenValidationParameters.ValidateLifetime = true;
                });

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<ForwardedHeadersOptions>(o => {
                o.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                o.KnownNetworks.Add(new IPNetwork(IPAddress.Parse("192.168.0.0"), 16));
                o.KnownNetworks.Add(new IPNetwork(IPAddress.Parse("10.0.0.0"), 8));
                o.KnownNetworks.Add(new IPNetwork(IPAddress.Parse("172.16.0.0"), 12));
            });

            services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimFilter.Clear();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var supportedCultures = new[] { "en-GB", "pl" };
            app.UseRequestLocalization(new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0]).AddSupportedCultures(supportedCultures).AddSupportedUICultures(supportedCultures));
            app.UseForwardedHeaders();

            app.UseMiddleware<ReturnUrlValidationMiddleware>();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseMiddleware<CustomErrorsHandlerMiddleware>();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<AccessTokenReaderMiddleware>();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static void SetupClients(IServiceCollection services)
        {
            var linksUrl = Environment.GetEnvironmentVariable("RINKUDESU_LINKS") ??
                           throw new InvalidOperationException(
                               "RINKUDESU_LINKS env variable pointing to links microservice must be set");
            var tagsUrl = Environment.GetEnvironmentVariable("RINKUDESU_TAGS") ?? throw new InvalidOperationException("RUNKUDESU_TAGS env variable pointing to tags microservice must be set");
            services.AddHttpClient<LinksClient>(o => {
                o.BaseAddress = new Uri(linksUrl);
            }).AddPolicyHandler(GetRetryPolicy());
            services.AddHttpClient<SharedLinksClient>(o => {
                o.BaseAddress = new Uri(linksUrl);
            }).AddPolicyHandler(GetRetryPolicy());
            services.AddHttpClient<TagsClient>(o => {
                o.BaseAddress = tagsUrl.ToUri();
            }).AddPolicyHandler(GetRetryPolicy());
            services.AddHttpClient<LinkTagsClient>(o => {
                o.BaseAddress = tagsUrl.ToUri();
            }).AddPolicyHandler(GetRetryPolicy());
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions.HandleTransientHttpError()
                .WaitAndRetryAsync(5, attempt => TimeSpan.FromSeconds(attempt));
        }

        private static void SetupKafka(IServiceCollection serviceCollection)
        {
            var kafkaConfig = KafkaConfigurationProvider.ReadFromEnv();
            serviceCollection.AddSingleton(kafkaConfig);
            serviceCollection.AddSingleton<IKafkaProducer, KafkaProducer>();
        }
    }
}
