namespace Rinkudesu.Gateways.Utils;

public static class EnvironmentalVariableReader
{
    public static bool RegistrationEnabled => string.IsNullOrEmpty(Environment.GetEnvironmentVariable("RINKUDESU_DISABLE_REGISTRATION"));
}
