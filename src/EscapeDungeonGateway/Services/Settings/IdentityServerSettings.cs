using System;

namespace EscapeDungeonGateway.Services
{
    public interface IIdentityServerSettings
    {
        string AuthorityUrl { get; set; }
        IdentityServerClientSettings EscapeDungeonClient { get; set; }
        IdentityServerClientSettings InteractiveClient { get; set; }
    }

    public class IdentityServerSettings: IIdentityServerSettings
    {
        public string AuthorityUrl { get; set; }

        public IdentityServerClientSettings EscapeDungeonClient { get; set; }
        public IdentityServerClientSettings InteractiveClient { get; set; }
    }

    public class IdentityServerClientSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string[] Scopes { get; set; }
    }
}
