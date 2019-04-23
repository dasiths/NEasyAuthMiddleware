using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NEasyAuthMiddleware.Core;

namespace NEasyAuthMiddleware.Sample
{
    public class CustomHeaderClaimMapper : IClaimMapper
    {
        public ClaimMapResult Map(IHeaderDictionary headers)
        {
            if (headers.ContainsKey("User-Agent"))
            {
                return ClaimMapResult.Success(new[]
                {
                    new Claim(ClaimTypes.System, headers["User-Agent"].First())
                });
            }

            return ClaimMapResult.NoResult;
        }
    }
}
