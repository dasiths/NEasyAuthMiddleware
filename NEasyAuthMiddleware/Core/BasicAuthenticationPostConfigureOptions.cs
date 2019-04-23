using System;
using Microsoft.Extensions.Options;

namespace NEasyAuthMiddleware.Core
{
    public class BasicAuthenticationPostConfigureOptions : IPostConfigureOptions<EasyAuthOptions>
    {
        public void PostConfigure(string name, EasyAuthOptions options)
        {
            if (string.IsNullOrEmpty(options.Realm))
            {
                throw new InvalidOperationException("Realm must be provided in options");
            }
        }
    }
}