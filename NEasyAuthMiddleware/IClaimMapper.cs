using Microsoft.AspNetCore.Http;

namespace NEasyAuthMiddleware
{
    public interface IClaimMapper
    {
        ClaimMapResult Map(HttpContext httpContext);
    }
}