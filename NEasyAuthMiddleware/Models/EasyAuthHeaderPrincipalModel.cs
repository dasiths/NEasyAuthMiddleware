using System.Text.Json.Serialization;

namespace NEasyAuthMiddleware.Models
{
    public class EasyAuthHeaderPrincipalModel
    {
        [JsonPropertyName("auth_typ")]
        public string AuthenticationType { get; set; }

        [JsonPropertyName("name_typ")]
        public string NameClaimType { get; set; }

        [JsonPropertyName("role_typ")]
        public string RoleClaimType { get; set; }

        [JsonPropertyName("claims")]
        public ClaimModel[] Claims { get; set; }
    }
}