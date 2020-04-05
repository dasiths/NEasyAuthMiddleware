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
        private readonly IHttpContextAccessor _contextAccessor;

        public CustomClaimMapper(IOptions<EasyAuthOptions> easyAuthOptions, IHttpContextAccessor contextAccessor)
        {
            _easyAuthOptions = easyAuthOptions;
            _contextAccessor = contextAccessor;
        }

        public ClaimMapResult Map(IHeaderDictionary headers)
        {
            var isIgnored =
                _easyAuthOptions.Value.IgnoreClaimTypes.Any(c =>
                    c.Equals(ClaimTypes.System, StringComparison.OrdinalIgnoreCase));

            // try and map the header claims from a value in the header

            if (!isIgnored && headers.ContainsKey(CustomHeaderDictionaryTransformer.HeaderName))
            {
                return ClaimMapResult.Success(new[]
                {
                    new Claim(ClaimTypes.System, headers[CustomHeaderDictionaryTransformer.HeaderName].First()),
                    new Claim(ClaimTypes.Webpage, _contextAccessor.HttpContext.Request.Path), 
                });
            }

            return ClaimMapResult.NoResult;
        }
    }
}
