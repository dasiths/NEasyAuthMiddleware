using System.Text.Json.Serialization;

namespace NEasyAuthMiddleware.Models
{
    public class ClaimModel
    {
        [JsonPropertyName("typ")]
        public string Type { get; set; }

        [JsonPropertyName("val")]
        public string Value { get; set; }
    }
}
