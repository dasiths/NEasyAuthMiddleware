namespace NEasyAuthMiddleware.Constants
{
    /// <summary>
    /// List of known providers for EasyAuth, taken from <see cref="https://learn.microsoft.com/en-us/azure/app-service/overview-authentication-authorization#identity-providers">official docs</see>
    /// </summary>
    public static class KnownAuthProviders
    {
        public const string Apple = "apple";
        public const string MicrosoftEntra = "aad";
        public const string Facebook = "facebook";
        public const string GitHub = "github";
        public const string Google = "google";
        public const string Twitter = "twitter";
    }
}
