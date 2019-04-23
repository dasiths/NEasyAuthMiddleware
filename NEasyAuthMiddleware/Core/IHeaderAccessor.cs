using Microsoft.AspNetCore.Http;

namespace NEasyAuthMiddleware.Core
{
    public interface IHeaderAccessor
    {
        IHeaderDictionary GetHeaders();
    }
}