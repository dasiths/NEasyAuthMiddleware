using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using NEasyAuthMiddleware.Core;

namespace NEasyAuthMiddleware.Sample
{
    public class CustomClaimsTransformer : IClaimsTransformer
    {
        public List<Claim> Transform(List<Claim> claims)
        {
            var filteredClaims = claims.Where(c => c.Type != ClaimTypes.System).ToList();

            if (claims.Any(c => c.Type == ClaimTypes.System 
                                && c.Value.IndexOf("chrome", StringComparison.OrdinalIgnoreCase) > -1))
            {
                filteredClaims.Add(new Claim(ClaimTypes.System, "Google Chrome"));
            }

            if (claims.Any(c => c.Type == ClaimTypes.System
                                && c.Value.IndexOf("windows", StringComparison.OrdinalIgnoreCase) > -1))
            {
                filteredClaims.Add(new Claim(ClaimTypes.System, "Microsoft Windows"));
            }

            return filteredClaims;
        }
    }
}