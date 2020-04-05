using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using NEasyAuthMiddleware.Constants;

namespace NEasyAuthMiddleware.Core
{
    public class EasyAuthOptions : AuthenticationSchemeOptions
    {
        public IList<string> IgnoreClaimTypes { get; set; } = new List<string>();
        public IList<string> ClaimTypesWithCommaSeparatedValues { get; set; } = new List<string>() { KnownEasyAuthClaimAliases.Roles };
    }
}
