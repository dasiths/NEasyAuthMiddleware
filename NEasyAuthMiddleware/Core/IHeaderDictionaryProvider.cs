using Microsoft.AspNetCore.Http;

namespace NEasyAuthMiddleware.Core
{
    public interface IHeaderDictionaryProvider
    {
        IHeaderDictionary GetHeaders();
    }
}