using System;
using System.Diagnostics.CodeAnalysis;

namespace Rinkudesu.Gateways.Webui.Models
{
    [ExcludeFromCodeCoverage]
    public class KeycloakSettings
    {
        private static KeycloakSettings? current;

        public static KeycloakSettings Current
        {
            get => current ?? throw new InvalidOperationException("Keycloak settings have not been initialised yet");
            set
            {
                if (current is not null)
                {
                    throw new InvalidOperationException("Keycloak settings have already been initialised");
                }

                current = value;
            }
        }

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
