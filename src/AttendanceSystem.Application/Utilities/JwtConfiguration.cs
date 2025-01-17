using AttendanceSystem.Application.Models;

namespace AttendanceSystem.Application.Utilities
{
    public class JwtConfiguration
    {
        public static JwtSettings GetJwtSettings()
        {
            string jwtKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? "";

            return new JwtSettings
            {
                Issuer = "orchestra.io",
                Audience = "api.orchestra.io",
                DurationInMinutes = 60,
                Key = jwtKey
            };
        }
    }
}