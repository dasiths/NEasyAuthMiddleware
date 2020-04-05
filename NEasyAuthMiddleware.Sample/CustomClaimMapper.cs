using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NEasyAuthMiddleware.Core;

namespace NEasyAuthMiddleware.Sample
{
    public class CustomClaimMapper : IClaimMapper
    {
        private readonly IOptions<EasyAuthOptions> _easyAuthOptions;

        public CustomClaimMapper(IOptions<EasyAuthOptions> easyAuthOptions)
        {
            _easyAuthOptions = easyAuthOptions;
        }

        public ClaimMapResult Map(IHeaderDictionary headers)
        {
            var isIgnored =
                _easyAuthOptions.Value.IgnoreClaimTypes.Any(c =>
                    c.Equals(ClaimTypes.Gender, StringComparison.OrdinalIgnoreCase));

            // try and map the header claims from a value in the header

            if (!isIgnored && headers.ContainsKey(CustomHeaderDictionaryTransformer.HeaderName))
            {
                return ClaimMapResult.Success(new[]
                {
                    new Claim(ClaimTypes.Gender, headers[CustomHeaderDictionaryTransformer.HeaderName].First())
                });
            }

            return ClaimMapResult.NoResult;
        }
    }
}
