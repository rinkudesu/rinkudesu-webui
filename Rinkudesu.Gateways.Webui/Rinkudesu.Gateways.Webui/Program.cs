using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Rinkudesu.Gateways.Webui.Models;

namespace Rinkudesu.Gateways.Webui
{
    public static class Program
    {
        public static KeycloakSettings KeycloakSettings { get; set; } = null!;
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}