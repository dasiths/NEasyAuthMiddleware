using Microsoft.AspNetCore.Authentication;

namespace NEasyAuthMiddleware.Core
{
    public class NEasyAuthOptions : AuthenticationSchemeOptions
    {
        public string Realm { get; set; }
    }
}
