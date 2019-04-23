using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NEasyAuthMiddleware.Core;
using NEasyAuthMiddleware.Mappers;

namespace NEasyAuthMiddleware
{
    public static class ServiceBuilderExtensions
    {
        public static AuthenticationBuilder AddEasyAuth(this AuthenticationBuilder builder, string authenticationScheme, Action<EasyAuthOptions> configureOptions)
        {
            builder.Services.AddSingleton<IHeaderAccessor, HeaderAccessor>();
            builder.Services.AddSingleton<IClaimMapper, StandardPrincipalClaimMapper>();
            builder.Services.AddSingleton<IPostConfigureOptions<EasyAuthOptions>, PostConfigureOptions<EasyAuthOptions>>();
            return builder.AddScheme<EasyAuthOptions, EasyAuthAuthenticationHandler>(
                authenticationScheme, configureOptions);
        }

        public static AuthenticationBuilder AddEasyAuth(this IServiceCollection collection, Action<EasyAuthOptions> configureOptions)
        {
            return collection.AddAuthentication(NEasyAuthDefaults.AuthenticationSchemeName)
                .AddEasyAuth(NEasyAuthDefaults.AuthenticationSchemeName, configureOptions);
        }
    }
}
