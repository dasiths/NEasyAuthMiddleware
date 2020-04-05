using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace NEasyAuthMiddleware.Core
{
    public interface IClaimsTransformer
    {
        List<Claim> Transform(List<Claim> claims);
    }
}
