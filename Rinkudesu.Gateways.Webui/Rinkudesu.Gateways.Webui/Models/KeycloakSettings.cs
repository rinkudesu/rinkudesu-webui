using System;

namespace Rinkudesu.Gateways.Webui.Models
{
    public class KeycloakSettings
    {
        public string Authority { get; }
        public string ClientId { get; }

        public KeycloakSettings()
        {
            Authority = Environment.GetEnvironmentVariable("RINKUDESU_AUTHORITY") ?? throw new InvalidOperationException("RINKUDESU_AUTHORITY must be set");
            ClientId = Environment.GetEnvironmentVariable("RINKUDESU_CLIENTID") ?? throw new InvalidOperationException("RINKUDESU_CLIENTID must be set");
        }

        public KeycloakSettings(string authority, string clientId)
        {
            Authority = authority;
            ClientId = clientId;
        }
    }
}