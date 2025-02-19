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

        public UserData? UserData { get; set; }
    }

    public class RefreshToken
    {
        public string Username { get; set; }
        public string TokenString { get; set; }
        public DateTime ExpiredAt { get; set; }
    }

    public class UserData
    {
        public Guid? UserId { get; set; }
        public Guid? GroupId { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? EmailAddress { get; set; }
        public string? UserType { get; set; }
        public string? GroupName { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }
}