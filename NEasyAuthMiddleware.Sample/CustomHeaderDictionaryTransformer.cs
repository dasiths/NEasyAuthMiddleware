using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NEasyAuthMiddleware.Core;

namespace NEasyAuthMiddleware.Sample
{
    public class CustomHeaderDictionaryTransformer: IHeaderDictionaryTransformer
    {
        public const string HeaderName = "User-Agent";

        public HeaderDictionary Transform(HeaderDictionary headerDictionary)
        {
            if (!headerDictionary.ContainsKey(HeaderName))
            {
                // adding default value
                headerDictionary.Add(HeaderName, "Google Chrome on Windows");
            }

            return headerDictionary;
        }
    }
}