using Newtonsoft.Json;

namespace AttendanceSystem.Application.Models
{
    public class AppSettings
    {
        public string ResourcePath { get; set; }
        public string TempResourcePath { get; set; }
    }


    [JsonObject("tokenSettings")]
    public class TokenSettings
    {
        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        [JsonProperty("audience")]
        public string Audience { get; set; }

        [JsonProperty("accessExpiration")]
        public int AccessExpiration { get; set; }

        [JsonProperty("refreshExpiration")]
        public int RefreshExpiration { get; set; }

    }
}