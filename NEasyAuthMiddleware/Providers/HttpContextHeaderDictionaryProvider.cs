using Microsoft.AspNetCore.Http;
using NEasyAuthMiddleware.Core;

namespace NEasyAuthMiddleware.Providers
{
    public class HttpContextHeaderDictionaryProvider : IHeaderDictionaryProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextHeaderDictionaryProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IHeaderDictionary GetHeaders()
        {
            return _httpContextAccessor.HttpContext.Request.Headers;
        }
    }
}
