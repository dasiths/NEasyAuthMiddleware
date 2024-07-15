using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using NEasyAuthMiddleware.Constants;

namespace NEasyAuthMiddleware.Core
{
    public class EasyAuthOptions : AuthenticationSchemeOptions
    {
        /// <summary>
        /// The provider to use for authentication. Default is <see cref="KnownAuthProviders.MicrosoftEntra"/>.
        /// </summary>
        public string Provider { get; set; } = KnownAuthProviders.MicrosoftEntra;
        public IList<string> IgnoreClaimTypes { get; set; } = new List<string>();
        public IList<string> ClaimTypesWithCommaSeparatedValues { get; set; } = new List<string>() { KnownEasyAuthClaimAliases.Roles };
    }
}
