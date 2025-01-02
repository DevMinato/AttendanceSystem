using AttendanceSystem.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceSystem.Domain.Entities
{
    [Table("Users")]
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// The user type. System, member
        /// </summary>
        public UserType UserType { get; set; }

        public DateTime? LastLoginDate { get; set; }
        public string SecurityQuestion1 { get; set; } = string.Empty;
        public string SecurityQuestion2 { get; set; } = string.Empty;
        public string SecurityQuestion3 { get; set; } = string.Empty;
        public string SecurityAnswer1 { get; set; } = string.Empty;
        public string SecurityAnswer2 { get; set; } = string.Empty;
        public string SecurityAnswer3 { get; set; } = string.Empty;
    }
}
