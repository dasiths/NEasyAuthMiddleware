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
            // remove the gender claim if it's the default
            return claims.Where(c => !(c.Type == ClaimTypes.Gender && 
                                     c.Value == CustomHeaderDictionaryTransformer.HeaderDefaultValue)).ToList();
        }
    }
}