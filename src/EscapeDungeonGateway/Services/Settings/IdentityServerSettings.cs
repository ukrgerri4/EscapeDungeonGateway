namespace EscapeDungeonGateway.Services
{
    public interface IIdentityServerSettings
    {
        string AuthorityUrl { get; set; }
        IdentityServerClientSettings EscapeDungeonClient { get; set; }
        IdentityServerClientSettings InteractiveClient { get; set; }
        IdentityServerResourceSettings EdGatewayResource { get; set; }
    }

    public class IdentityServerSettings : IIdentityServerSettings
    {
        public string AuthorityUrl { get; set; }

        public IdentityServerClientSettings EscapeDungeonClient { get; set; }
        public IdentityServerClientSettings InteractiveClient { get; set; }
        public IdentityServerResourceSettings EdGatewayResource { get; set; }
    }

    public class IdentityServerClientSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string[] Scopes { get; set; }
    }

    public class IdentityServerResourceSettings
    {
        public string Name { get; set; }
        public string Secret { get; set; }
    }
}
