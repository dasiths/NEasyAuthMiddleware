using Microsoft.AspNetCore.Authentication;

namespace NEasyAuthMiddleware.Core
{
    public class EasyAuthOptions : AuthenticationSchemeOptions
    {
        public string Realm { get; set; }
    }
}
