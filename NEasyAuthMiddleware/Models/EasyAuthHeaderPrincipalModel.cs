using Newtonsoft.Json;

namespace NEasyAuthMiddleware.Models
{
    public class EasyAuthHeaderPrincipalModel
    {
        [JsonProperty("auth_typ")]
        public string AuthenticationType { get; set; }

        [JsonProperty("name_typ")]
        public string NameClaimType { get; set; }

        [JsonProperty("role_typ")]
        public string RoleClaimType { get; set; }

        [JsonProperty("claims")]
        public ClaimModel[] Claims { get; set; }
    }
}