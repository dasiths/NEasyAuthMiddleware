using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using NEasyAuthMiddleware.Models;

namespace NEasyAuthMiddleware.Providers
{
    public class JsonProvider : IHeaderDictionaryProvider
    {
        private readonly JsonProviderOptions _jsonProviderOptions;
        private readonly ILogger<JsonProvider> _logger;

        public JsonProvider(JsonProviderOptions jsonProviderOptions, ILogger<JsonProvider> logger)
        {
            _jsonProviderOptions = jsonProviderOptions;
            _logger = logger;
        }

        public IHeaderDictionary GetHeaders()
        {
            var headerDictionary = new HeaderDictionary();
            var contents = File.ReadAllText(_jsonProviderOptions.JsonFilePath);
            _logger.LogDebug($"Reading claims in file: {_jsonProviderOptions.JsonFilePath}");

            var payload = JsonConvert.DeserializeObject<AuthMeJsonPrincipalModel[]>(contents);

            if (payload.Any())
            {
                var target = payload.First();
                var result = new HeaderPrincipalModel()
                {
                    AuthenticationType = target.AuthenticationType,
                    NameClaimType = ClaimTypes.Name,
                    RoleClaimType = ClaimTypes.Role,
                    Claims = target.Claims
                };
                var json = JsonConvert.SerializeObject(result);
                var encodedString = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
                headerDictionary.Add(new KeyValuePair<string, StringValues>(HeaderConstants.PrincipalObjectHeader, encodedString));

                _logger.LogDebug($"Found {result.Claims.Length} claims in file.");
            }

            return headerDictionary;
        }
    }
}