using EscapeDungeonGateway.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EscapeDungeonGateway.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IIdentityServerSettings>(p => configuration.GetSection("IdentityServer").Get<IdentityServerSettings>());
            return services;
        }
    }
}
