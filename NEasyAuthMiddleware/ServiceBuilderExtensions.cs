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
        // https://joonasw.net/view/creating-auth-scheme-in-aspnet-core-2

        public static AuthenticationBuilder AddEasyAuth(this AuthenticationBuilder builder, string authenticationScheme, Action<NEasyAuthOptions> configureOptions)
        {
            return builder.AddScheme<NEasyAuthOptions, NEasyAuthAuthenticationHandler>(
                authenticationScheme, configureOptions);
        }

        public static AuthenticationBuilder AddEasyAuth(this IServiceCollection collection, Action<NEasyAuthOptions> configureOptions)
        {
            collection.AddScoped<IClaimMapper, StandardPrincipalClaimMapper>();
            return collection.AddAuthentication(NEasyAuthDefaults.AuthenticationSchemeName)
                .AddEasyAuth(NEasyAuthDefaults.AuthenticationSchemeName, configureOptions);
        }
    }
}
