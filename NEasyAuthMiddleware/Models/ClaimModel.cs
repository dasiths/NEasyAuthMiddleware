using Newtonsoft.Json;

namespace NEasyAuthMiddleware.Models
{
    public class ClaimModel
    {
        [JsonProperty("typ")]
        public string Type { get; set; }

        [JsonProperty("val")]
        public string Value { get; set; }
    }
}
