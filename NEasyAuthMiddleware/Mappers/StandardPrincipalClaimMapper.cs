using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NEasyAuthMiddleware.Core;
using NEasyAuthMiddleware.Models;
using NEasyAuthMiddleware.Providers;

namespace NEasyAuthMiddleware.Mappers
{
    public class StandardPrincipalClaimMapper : IClaimMapper
    {
        private readonly ILogger<StandardPrincipalClaimMapper> _logger;

        public StandardPrincipalClaimMapper(ILogger<StandardPrincipalClaimMapper> logger)
        {
            _logger = logger;
        }

        public ClaimMapResult Map(IHeaderDictionary headers)
        {
            if (!string.IsNullOrEmpty(headers[HeaderConstants.PrincipalObjectHeader]))
            {
                var claims = new List<Claim>();
                _logger.LogInformation($"Building claims from payload in {HeaderConstants.PrincipalObjectHeader} header.");

                try
                {
                    var headerValue = headers[HeaderConstants.PrincipalObjectHeader].First();
                    var payload = Encoding.UTF8.GetString(Convert.FromBase64String(headerValue));
                    var headerPrincipalModel = JsonConvert.DeserializeObject<HeaderPrincipalModel>(payload);
                   
                    foreach (var claimsModel in headerPrincipalModel.Claims)
                    {
                        var claimType = claimsModel.Type;

                        if (claimsModel.Type.Equals("roles", StringComparison.OrdinalIgnoreCase))
                        {
                            claimType = string.IsNullOrEmpty(headerPrincipalModel.RoleClaimType) ? ClaimTypes.Role : headerPrincipalModel.RoleClaimType;
                        }else if (claimsModel.Type.Equals("name", StringComparison.OrdinalIgnoreCase))
                        {
                            claimType = string.IsNullOrEmpty(headerPrincipalModel.NameClaimType) ? ClaimTypes.Name : headerPrincipalModel.NameClaimType;
                        }

                        claims.AddRange(claimsModel.Value.Split(',')
                            .Select(c => new Claim(claimType, c)));
                    }

                    return ClaimMapResult.Success(claims);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Building claims from header failed");
                }
            }

            return ClaimMapResult.Fail($"{HeaderConstants.PrincipalObjectHeader} header was not present or in the expected format.");
        }
    }
}
