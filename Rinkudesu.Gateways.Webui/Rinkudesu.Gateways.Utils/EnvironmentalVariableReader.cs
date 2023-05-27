using System.Diagnostics.CodeAnalysis;

namespace Rinkudesu.Gateways.Utils;

[ExcludeFromCodeCoverage]
public static class EnvironmentalVariableReader
{
    public static bool RegistrationEnabled => string.IsNullOrEmpty(Environment.GetEnvironmentVariable("RINKUDESU_DISABLE_REGISTRATION"));
}
