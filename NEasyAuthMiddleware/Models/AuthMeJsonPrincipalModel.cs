using System.Text.Json.Serialization;

namespace NEasyAuthMiddleware.Models
{
    public class AuthMeJsonPrincipalModel
    {
        [JsonPropertyName("provider_name")]
        public string AuthenticationType { get; set; }

        [JsonPropertyName("id_token")]
        public string IdToken { get; set; }

        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("user_claims")]
        public ClaimModel[] Claims { get; set; }
    }
}