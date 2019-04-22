using System;
using System.Security.Claims;

namespace NEasyAuthMiddleware
{
    public class ClaimMapResult
    {
        public readonly ClaimMapResultType Type;
        public readonly Claim Claim;
        public readonly string ResultMessage;

        public ClaimMapResult(ClaimMapResultType type, Claim claim = null, string resultMessage = "")
        {
            Claim = claim;
            Type = type;
            ResultMessage = resultMessage;
        }

        public static ClaimMapResult NoResult => new ClaimMapResult(ClaimMapResultType.NoResult);
        public static Func<string, ClaimMapResult> Fail => message => new ClaimMapResult(ClaimMapResultType.Fail, null, message);
        public static Func<Claim,ClaimMapResult> Success => claim => new ClaimMapResult(ClaimMapResultType.Success, claim);
    }
}
