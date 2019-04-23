using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NEasyAuthMiddleware.Core
{
    public class EasyAuthAuthenticationHandler : AuthenticationHandler<EasyAuthOptions>
    {
        private readonly IHeaderAccessor _headerAccessor;
        private readonly IList<IClaimMapper> _claimMappers;
        private readonly ILogger<EasyAuthAuthenticationHandler> _logger;

        public EasyAuthAuthenticationHandler(
            IHeaderAccessor headerAccessor,
            IEnumerable<IClaimMapper> claimMappers,
            IOptionsMonitor<EasyAuthOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _headerAccessor = headerAccessor;
            _claimMappers = claimMappers.ToList();
            _logger = logger.CreateLogger<EasyAuthAuthenticationHandler>();
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var allResults = _claimMappers
                .Select(m => m.Map(_headerAccessor.GetHeaders()))
                .ToList();

            var failedResults = allResults
                .Where(c => c.Type == ClaimMapResultType.Fail)
                .ToList();

            var successfulResults = allResults
                .Where(c => c.Type == ClaimMapResultType.Success)
                .ToList();

            if (failedResults.Any())
            {
                _logger.LogDebug($"{nameof(EasyAuthAuthenticationHandler)} found {failedResults.Count} result(s) that have a failed status.");
                var messages = failedResults
                    .Select(c => c.ResultMessage);
                return Task.FromResult(AuthenticateResult.Fail(string.Join("\n", messages)));
            }

            if (successfulResults.Any())
            {
                var claims = successfulResults
                    .SelectMany(c => c.Claims)
                    .ToList();

                _logger.LogDebug($"{nameof(EasyAuthAuthenticationHandler)} found {claims.Count} successful result(s) and mapped them to claims.");

                var identity = new ClaimsIdentity(claims);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                _logger.LogDebug($"{nameof(EasyAuthAuthenticationHandler)} constructed the {nameof(ClaimsPrincipal)} successfully.");
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            _logger.LogDebug($"{nameof(EasyAuthAuthenticationHandler)} did not find any successful results.");
            return Task.FromResult(AuthenticateResult.NoResult());
        }
    }
}