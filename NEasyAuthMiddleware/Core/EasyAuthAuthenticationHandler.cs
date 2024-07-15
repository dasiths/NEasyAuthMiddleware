using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NEasyAuthMiddleware.Core
{
    public class EasyAuthAuthenticationHandler : SignOutAuthenticationHandler<EasyAuthOptions>
    {
        private readonly IList<IHeaderDictionaryProvider> _headerDictionaryProviders;
        private readonly IList<IClaimMapper> _claimMappers;
        private readonly IList<IClaimsTransformer> _claimsTransformers;
        private readonly IList<IHeaderDictionaryTransformer> _headerDictionaryTransformers;
        private readonly ILogger<EasyAuthAuthenticationHandler> _logger;

        public EasyAuthAuthenticationHandler(IEnumerable<IHeaderDictionaryProvider> headerDictionaryProviders,
            IEnumerable<IHeaderDictionaryTransformer> headerDictionaryTransformers,
            IEnumerable<IClaimMapper> claimMappers,
            IEnumerable<IClaimsTransformer> claimsTransformers,
            IOptionsMonitor<EasyAuthOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder) : base(options, logger, encoder)
        {
            _claimsTransformers = claimsTransformers.ToList();
            _headerDictionaryTransformers = headerDictionaryTransformers.ToList();
            _headerDictionaryProviders = headerDictionaryProviders.ToList();
            _claimMappers = claimMappers.ToList();
            _logger = logger.CreateLogger<EasyAuthAuthenticationHandler>();
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var allHeaders = _headerDictionaryProviders
                .SelectMany(p => p.GetHeaders())
                .Aggregate((IHeaderDictionary)new HeaderDictionary(),
                (dictionary, pair) =>
                {
                    dictionary.Add(pair);
                    return dictionary;
                });

            allHeaders =
                _headerDictionaryTransformers
                    .Reverse()
                    .Aggregate(allHeaders, (hd, t) => t.Transform(hd));

            var allResults = _claimMappers
                .Select(m => m.Map(allHeaders))
                .ToList();

            var failedResults = allResults
                .Where(c => c.Type == ClaimMapResultType.Fail)
                .ToList();

            var successfulResults = allResults
                .Where(c => c.Type == ClaimMapResultType.Success)
                .ToList();

            if (failedResults.Any())
            {
                _logger.LogTrace($"{nameof(EasyAuthAuthenticationHandler)} found {failedResults.Count} result(s) that have a failed status.");
                var messages = failedResults
                    .Select(c => c.ResultMessage);
                return Task.FromResult(AuthenticateResult.Fail(string.Join("\n", messages)));
            }

            if (successfulResults.Any())
            {
                var claims = successfulResults
                    .SelectMany(c => c.Claims)
                    .ToList();

                claims = _claimsTransformers
                    .Reverse()
                    .Aggregate(claims, (c, t) => t.Transform(c));

                _logger.LogTrace($"{nameof(EasyAuthAuthenticationHandler)} found {claims.Count} successful result(s) and mapped them to claims.");

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                _logger.LogTrace($"{nameof(EasyAuthAuthenticationHandler)} constructed the {nameof(ClaimsPrincipal)} successfully.");
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            _logger.LogTrace($"{nameof(EasyAuthAuthenticationHandler)} did not find any successful results.");
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties) 
        {
            Response.Redirect($"/.auth/login/{Options.Provider}?post_login_redirect_uri={Request.GetEncodedPathAndQuery()}");
            return Task.CompletedTask;
        }

        protected override Task HandleSignOutAsync(AuthenticationProperties properties)
        {
            Response.Redirect("/.auth/logout");
            return Task.CompletedTask;
        }
    }
}