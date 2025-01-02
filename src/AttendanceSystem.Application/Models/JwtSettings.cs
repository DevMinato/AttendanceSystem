﻿namespace AttendanceSystem.Application.Models
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int DurationInMinutes { get; set; }
        public string Key { get; set; }
    }
}