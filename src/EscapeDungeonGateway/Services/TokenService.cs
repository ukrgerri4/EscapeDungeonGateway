using EscapeDungeonGateway.Infrastracture;
using EscapeDungeonGateway.Models;
using IdentityModel.Client;
using System.Net.Http;
using System.Threading.Tasks;

namespace EscapeDungeonGateway.Services
{
    public interface ITokenService
    {
        Task<Result<Token>> GetEscapeDungeonTokenAsync(string login, string password);
    }

    public class TokenService : ITokenService
    {
        private readonly HttpClient httpClient;
        private readonly IIdentityServerSettings identityServerSettings;

        public TokenService(HttpClient httpClient, IIdentityServerSettings identityServerSettings)
        {
            this.httpClient = httpClient;
            this.identityServerSettings = identityServerSettings;
        }

        public async Task<Result<Token>> GetEscapeDungeonTokenAsync(string login, string password)
        {
            var discoveryResponse = await httpClient
                .GetDiscoveryDocumentAsync(
                    new DiscoveryDocumentRequest
                    { 
                        Policy = { RequireHttps = false }
                    }
                );

            if (discoveryResponse.IsError) { return Result.Fail<Token>(discoveryResponse.Error); }

            var client = identityServerSettings.EscapeDungeonClient;

            var tokenResponse = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = discoveryResponse.TokenEndpoint,
                ClientId = client.ClientId,
                ClientSecret = client.ClientSecret,

                UserName = login,
                Password = password,

                Scope = string.Join(" ", client.Scopes)
            });

            if (tokenResponse.IsError) { return Result.Fail<Token>(tokenResponse.Error); }

            return Result.Ok(new Token { AccessToken = tokenResponse.AccessToken });
        }
    }
}
