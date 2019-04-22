using Microsoft.AspNetCore.Authentication;

namespace NEasyAuthMiddleware
{
    public class NEasyAuthOptions : AuthenticationSchemeOptions
    {
        public string Realm { get; set; }
    }
}
