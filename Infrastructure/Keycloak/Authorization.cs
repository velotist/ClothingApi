using System;
using ClothingApi.Infrastructure.Keycloak;
using Microsoft.AspNetCore.Authorization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class KeycloakAuthorizationServiceCollectionExtensions
    {
        public static IServiceCollection AddKeycloakAuthorization(this IServiceCollection services, Action<ClothingApi.Infrastructure.Keycloak.AuthorizationOptions> configure)
        {
            services.Configure(configure);
            services.AddHttpContextAccessor();
            services.AddSingleton<IAuthorizationHandler, AuthorizationHandler>();

            return services;
        }
    }
}