using System.Text.Json.Serialization;

namespace AttendanceSystem.Application.Models
{
    public class JwtAuthResult
    {
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }
        [JsonPropertyName("refreshToken")]
        public RefreshToken? RefreshToken { get; set; }
        [JsonPropertyName("expiredAt")]
        public DateTime ExpirationTime { get; set; }
    }

    public class RefreshToken
    {
        public string Username { get; set; }
        public string TokenString { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}