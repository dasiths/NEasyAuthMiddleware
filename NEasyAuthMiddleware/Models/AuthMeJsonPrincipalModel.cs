using Newtonsoft.Json;

namespace NEasyAuthMiddleware.Models
{
    public class AuthMeJsonPrincipalModel
    {
        [JsonProperty("provider_name")]
        public string AuthenticationType { get; set; }

        [JsonProperty("id_token")]
        public string IdToken { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("user_claims")]
        public ClaimModel[] Claims { get; set; }
    }
}