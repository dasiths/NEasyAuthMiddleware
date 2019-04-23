using Microsoft.AspNetCore.Http;

namespace NEasyAuthMiddleware.Providers
{
    public interface IHeaderDictionaryProvider
    {
        IHeaderDictionary GetHeaders();
    }
}