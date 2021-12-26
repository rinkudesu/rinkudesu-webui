namespace Rinkudesu.Gateways.Clients;

public interface IAuthorisedMicroserviceClient<out TClient>
{
    TClient SetAccessToken(string token);
}