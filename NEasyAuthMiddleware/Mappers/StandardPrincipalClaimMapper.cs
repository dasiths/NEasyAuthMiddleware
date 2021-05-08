using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NEasyAuthMiddleware.Constants;
using NEasyAuthMiddleware.Core;
using NEasyAuthMiddleware.Models;
using NEasyAuthMiddleware.Providers;

namespace NEasyAuthMiddleware.Mappers
{
    public class StandardPrincipalClaimMapper : IClaimMapper
    {
        private readonly ILogger<StandardPrincipalClaimMapper> _logger;
        private readonly IOptions<EasyAuthOptions> _easyAuthOptions;

        public StandardPrincipalClaimMapper(ILogger<StandardPrincipalClaimMapper> logger, IOptions<EasyAuthOptions> easyAuthOptions)
        {
            _logger = logger;
            _easyAuthOptions = easyAuthOptions;
        }

        public ClaimMapResult Map(IHeaderDictionary headers)
        {
            if (!string.IsNullOrEmpty(headers[KnownEasyAuthHeaders.PrincipalObjectHeader]))
            {
                var claims = new List<Claim>();
                _logger.LogTrace($"Building claims from payload in {KnownEasyAuthHeaders.PrincipalObjectHeader} header.");

                try
                {
                    var headerValue = headers[KnownEasyAuthHeaders.PrincipalObjectHeader].First();
                    var payload = Encoding.UTF8.GetString(Convert.FromBase64String(headerValue));
                    var headerPrincipalModel = JsonSerializer.Deserialize<EasyAuthHeaderPrincipalModel>(payload);

                    var config = _easyAuthOptions.Value;

                    foreach (var claimsModel in headerPrincipalModel.Claims)
                    {
                        var claimType = claimsModel.Type;
                        var ignoreClaimType = config.IgnoreClaimTypes.Any(c =>
                            c.Equals(claimType, StringComparison.OrdinalIgnoreCase));

                        if (!ignoreClaimType)
                        {
                            var commaSeparated =
                                config.ClaimTypesWithCommaSeparatedValues.Any(c =>
                                    c.Equals(claimType, StringComparison.OrdinalIgnoreCase));

                            _logger.LogTrace($"ClaimType \"{claimType}\" value is treated as comma separated = {commaSeparated}");

                            if (claimsModel.Type.Equals(KnownEasyAuthClaimAliases.Roles, StringComparison.OrdinalIgnoreCase))
                            {
                                claimType = string.IsNullOrEmpty(headerPrincipalModel.RoleClaimType) ? ClaimTypes.Role : headerPrincipalModel.RoleClaimType;
                            }
                            else if (claimsModel.Type.Equals(KnownEasyAuthClaimAliases.Name, StringComparison.OrdinalIgnoreCase))
                            {
                                claimType = string.IsNullOrEmpty(headerPrincipalModel.NameClaimType) ? ClaimTypes.Name : headerPrincipalModel.NameClaimType;
                            }

                            if (commaSeparated)
                            {
                                claims.AddRange(claimsModel.Value.Split(',')
                                    .Select(c => new Claim(claimType, c)));
                            }
                            else
                            {
                                claims.Add(new Claim(claimType, claimsModel.Value));
                            }
                        }
                        else
                        {
                            _logger.LogTrace($"Ignoring ClaimType: {claimType}");
                        }
                    }

                    return ClaimMapResult.Success(claims);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Building claims from header failed");
                }
            }

            return ClaimMapResult.Fail($"{KnownEasyAuthHeaders.PrincipalObjectHeader} header was not present or in the expected format.");
        }
    }
}
