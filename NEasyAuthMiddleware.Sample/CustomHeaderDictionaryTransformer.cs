using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NEasyAuthMiddleware.Core;

namespace NEasyAuthMiddleware.Sample
{
    public class CustomHeaderDictionaryTransformer: IHeaderDictionaryTransformer
    {
        public const string HeaderName = "x-user-gender";
        public const string HeaderDefaultValue = "Not Provided";

        public HeaderDictionary Transform(HeaderDictionary headerDictionary)
        {
            if (!headerDictionary.ContainsKey(HeaderName))
            {
                // adding default gender
                headerDictionary.Add(HeaderName, (new Random()).Next(100) > 50 ? HeaderDefaultValue : "NonBinary");
            }

            return headerDictionary;
        }
    }
}