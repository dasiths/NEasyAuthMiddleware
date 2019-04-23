using Microsoft.AspNetCore.Http;

namespace NEasyAuthMiddleware.Core
{
    public class HeaderAccessor : IHeaderAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HeaderAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IHeaderDictionary GetHeaders()
        {
            return _httpContextAccessor.HttpContext.Request.Headers;
        }
    }
}
