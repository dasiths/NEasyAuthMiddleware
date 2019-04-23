using Microsoft.AspNetCore.Http;

namespace NEasyAuthMiddleware.Core
{
    public interface IClaimMapper
    {
        ClaimMapResult Map(IHeaderDictionary headers);
    }
}