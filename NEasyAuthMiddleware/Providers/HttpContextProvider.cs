using Microsoft.AspNetCore.Http;

namespace NEasyAuthMiddleware.Providers
{
    public class HttpContextProvider : IHeaderDictionaryProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IHeaderDictionary GetHeaders()
        {
            return _httpContextAccessor.HttpContext.Request.Headers;
        }
    }
}
