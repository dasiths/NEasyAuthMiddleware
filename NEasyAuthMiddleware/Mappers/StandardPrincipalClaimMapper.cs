﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NEasyAuthMiddleware.Core;

namespace NEasyAuthMiddleware.Mappers
{
    public class StandardPrincipalClaimMapper : IClaimMapper
    {
        private class ClaimModel
        {
            [JsonProperty("typ")]
            public string Type { get; set; }

            [JsonProperty("val")]
            public string Value { get; set; }
        }

        private class PrincipalModel
        {
            [JsonProperty("claims")]
            public ClaimModel[] Claims { get; set; }
        }

        private const string PrincipalObjectHeader = "X-MS-CLIENT-PRINCIPAL";
        private const string PrincipalNameHeader = "X-MS-CLIENT-PRINCIPAL-NAME";
        private const string PrincipalIdpHeaderName = "X-MS-CLIENT-PRINCIPAL-IDP";

        private readonly ILogger<StandardPrincipalClaimMapper> _logger;

        public StandardPrincipalClaimMapper(ILogger<StandardPrincipalClaimMapper> logger)
        {
            _logger = logger;
        }

        public ClaimMapResult Map(IHeaderDictionary headers)
        {
            if (!string.IsNullOrEmpty(headers[PrincipalObjectHeader]))
            {
                var claims = new List<Claim>();
                _logger.LogInformation($"Building claims from payload in {PrincipalObjectHeader} header.");
                var xMsClientPrincipal = JsonConvert.DeserializeObject<PrincipalModel>(headers[PrincipalObjectHeader][0]);
                var claimsModels = xMsClientPrincipal.Claims;

                foreach (var claimsModel in claimsModels)
                {
                    var claimType = claimsModel.Type;
                    if (claimsModel.Type.Equals("roles", StringComparison.OrdinalIgnoreCase))
                    {
                        claimType = ClaimTypes.Role;
                    }

                    claims.AddRange(claimsModel.Value.Split(',')
                        .Select(c => new Claim(claimType, c)));
                }

                return ClaimMapResult.Success(claims);
            }

            return ClaimMapResult.Fail($"{PrincipalObjectHeader} header was not present.");
        }
    }
}