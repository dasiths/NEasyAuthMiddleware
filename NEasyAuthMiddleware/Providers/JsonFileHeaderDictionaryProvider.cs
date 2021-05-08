﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using NEasyAuthMiddleware.Constants;
using NEasyAuthMiddleware.Core;
using NEasyAuthMiddleware.Models;

namespace NEasyAuthMiddleware.Providers
{
    public class JsonFileHeaderDictionaryProvider : IHeaderDictionaryProvider
    {
        private readonly JsonFileHeaderDictionaryProviderOptions _jsonFileHeaderDictionaryProviderOptions;
        private readonly ILogger<JsonFileHeaderDictionaryProvider> _logger;

        public JsonFileHeaderDictionaryProvider(JsonFileHeaderDictionaryProviderOptions jsonFileHeaderDictionaryProviderOptions, ILogger<JsonFileHeaderDictionaryProvider> logger)
        {
            _jsonFileHeaderDictionaryProviderOptions = jsonFileHeaderDictionaryProviderOptions;
            _logger = logger;
        }

        public IHeaderDictionary GetHeaders()
        {
            var headerDictionary = new HeaderDictionary();
            var contents = File.ReadAllText(_jsonFileHeaderDictionaryProviderOptions.JsonFilePath);
            _logger.LogTrace($"Reading claims in file: {_jsonFileHeaderDictionaryProviderOptions.JsonFilePath}");

            var payload = JsonSerializer.Deserialize<AuthMeJsonPrincipalModel[]>(contents);

            if (payload.Any())
            {
                var target = payload.First();
                var result = new EasyAuthHeaderPrincipalModel()
                {
                    AuthenticationType = target.AuthenticationType,
                    NameClaimType = ClaimTypes.Name,
                    RoleClaimType = ClaimTypes.Role,
                    Claims = target.Claims
                };
                var json = JsonSerializer.Serialize(result);
                var encodedString = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
                headerDictionary.Add(new KeyValuePair<string, StringValues>(KnownEasyAuthHeaders.PrincipalObjectHeader, encodedString));

                _logger.LogTrace($"Found {result.Claims.Length} claims in file.");
            }

            return headerDictionary;
        }
    }
}