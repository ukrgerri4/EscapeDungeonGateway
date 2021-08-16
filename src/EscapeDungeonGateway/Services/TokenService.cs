using EscapeDungeonGateway.Infrastracture;
using EscapeDungeonGateway.Models;
using IdentityModel.Client;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EscapeDungeonGateway.Services
{
    public interface ITokenService
    {
        Task<Result<Token>> GetEscapeDungeonTokenAsync(string login, string password);
        Task<Result<Introspect>> IntrospectEscapeDungeonAsync(string token);
        Task<Result<string>> UserInfoAsync(string token);
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
            var discoveryResponse = await Discovery();
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

        public async Task<Result<Introspect>> IntrospectEscapeDungeonAsync(string token)
        {
            var discoveryResponse = await Discovery();
            if (discoveryResponse.IsError) { return Result.Fail<Introspect>(discoveryResponse.Error); }
            var client = identityServerSettings.EscapeDungeonClient;
            
            var introspectResponse = await httpClient.IntrospectTokenAsync(new TokenIntrospectionRequest
            {
                Address = discoveryResponse.IntrospectionEndpoint,
                ClientId = "ed_gateway",
                ClientSecret = "ScopeSecret",
                ClientCredentialStyle = ClientCredentialStyle.AuthorizationHeader,

                Token = token
            });
            
            if (introspectResponse.IsError) { return Result.Fail<Introspect>(introspectResponse.Error); }

            var introspectResponseData = JsonConvert.DeserializeObject<Introspect>(introspectResponse.Raw);
            return Result.Ok(introspectResponseData);
        }

        public async Task<Result<string>> UserInfoAsync(string token)
        {
            var discoveryResponse = await Discovery();
            if (discoveryResponse.IsError) { return Result.Fail<string>(discoveryResponse.Error); }
            
            var userInfoResponse = await httpClient.GetUserInfoAsync(new UserInfoRequest
            {
                Address = discoveryResponse.UserInfoEndpoint,
                Token = token
            });
            
            if (userInfoResponse.IsError) { return Result.Fail<string>(userInfoResponse.Error); }

            return Result.Ok(userInfoResponse.Raw);
        }

        private async Task<DiscoveryDocumentResponse> Discovery()
        {
            return await httpClient
                .GetDiscoveryDocumentAsync(
                    new DiscoveryDocumentRequest
                    { 
                        Policy = { RequireHttps = false }
                    }
                );
        } 
    }
}
