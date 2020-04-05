using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using NEasyAuthMiddleware.Core;
using NEasyAuthMiddleware.Mappers;
using NEasyAuthMiddleware.Providers;

namespace NEasyAuthMiddleware
{
    public static class ServiceBuilderExtensions
    {
        public static AuthenticationBuilder AddEasyAuth(this AuthenticationBuilder builder, string authenticationScheme, Action<EasyAuthOptions> configure = null)
        {
            builder.Services.AddSingleton<IHeaderDictionaryProvider, HttpContextHeaderDictionaryProvider>();
            builder.Services.AddSingleton<IClaimMapper, StandardPrincipalClaimMapper>();

            if (configure != null)
            {
                builder.Services.Configure<EasyAuthOptions>(configure);
            }

            return builder.AddScheme<EasyAuthOptions, EasyAuthAuthenticationHandler>(
                authenticationScheme, _ => {});
        }

        public static AuthenticationBuilder AddEasyAuth(this IServiceCollection collection, Action<EasyAuthOptions> configure = null)
        {
            return collection.AddAuthentication(NEasyAuthDefaults.AuthenticationSchemeName)
                .AddEasyAuth(NEasyAuthDefaults.AuthenticationSchemeName, configure);
        }

        public static void UseJsonFileToMockEasyAuth(this IServiceCollection collection, string fileName)
        {
            collection.AddSingleton(provider => new JsonFileHeaderDictionaryProviderOptions()
            {
                JsonFilePath = fileName
            });
            collection.AddSingleton<IHeaderDictionaryProvider, JsonFileHeaderDictionaryProvider>();
        }
    }
}
