using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using NEasyAuthMiddleware.Core;
using NEasyAuthMiddleware.Mappers;
using NEasyAuthMiddleware.Providers;

namespace NEasyAuthMiddleware
{
    public static class ServiceBuilderExtensions
    {
        public static AuthenticationBuilder AddEasyAuth(this AuthenticationBuilder builder, string authenticationScheme)
        {
            builder.Services.AddSingleton<IHeaderDictionaryProvider, HttpContextProvider>();
            builder.Services.AddSingleton<IClaimMapper, StandardPrincipalClaimMapper>();
            return builder.AddScheme<EasyAuthOptions, EasyAuthAuthenticationHandler>(
                authenticationScheme, _ => {});
        }

        public static AuthenticationBuilder AddEasyAuth(this IServiceCollection collection)
        {
            return collection.AddAuthentication(NEasyAuthDefaults.AuthenticationSchemeName)
                .AddEasyAuth(NEasyAuthDefaults.AuthenticationSchemeName);
        }

        public static void UseJsonFileToMockEasyAuth(this IServiceCollection collection, string fileName)
        {
            collection.AddSingleton(provider => new JsonProviderOptions()
            {
                JsonFilePath = fileName
            });
            collection.AddSingleton<IHeaderDictionaryProvider, JsonProvider>();
        }
    }
}
