using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace NEasyAuthMiddleware.Core
{
    public interface IHeaderDictionaryTransformer
    {
        IHeaderDictionary Transform(IHeaderDictionary headerDictionary);
    }
}
