using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace NEasyAuthMiddleware.Core
{
    public class ClaimMapResult
    {
        public readonly ClaimMapResultType Type;
        public readonly IList<Claim> Claims;
        public readonly string ResultMessage;

        public ClaimMapResult(ClaimMapResultType type, IList<Claim> claims = null, string resultMessage = "")
        {
            Claims = claims;
            Type = type;
            ResultMessage = resultMessage;
        }

        public static ClaimMapResult NoResult => new ClaimMapResult(ClaimMapResultType.NoResult);
        public static Func<string, ClaimMapResult> Fail => message => new ClaimMapResult(ClaimMapResultType.Fail, null, message);
        public static Func<IEnumerable<Claim>,ClaimMapResult> Success => claims => new ClaimMapResult(ClaimMapResultType.Success, claims.ToList());
    }
}
