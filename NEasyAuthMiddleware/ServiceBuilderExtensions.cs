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
        public static AuthenticationBuilder AddEasyAuth(this AuthenticationBuilder builder, string authenticationScheme)
        {
            builder.Services.AddSingleton<IHeaderAccessor, HeaderAccessor>();
            builder.Services.AddSingleton<IClaimMapper, StandardPrincipalClaimMapper>();
            return builder.AddScheme<EasyAuthOptions, EasyAuthAuthenticationHandler>(
                authenticationScheme, _ => {});
        }

        public static AuthenticationBuilder AddEasyAuth(this IServiceCollection collection)
        {
            return collection.AddAuthentication(NEasyAuthDefaults.AuthenticationSchemeName)
                .AddEasyAuth(NEasyAuthDefaults.AuthenticationSchemeName);
        }
    }
}
