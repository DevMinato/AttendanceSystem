using AttendanceSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceSystem.Domain.Entities
{
    [Table("Members", Schema = "RS")]
    public class Member : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public GenderEnum Gender { get; set; }
        public MemberType MemberType { get; set; }
        public Guid? DisciplerId { get; set; }
        public Guid FellowshipId { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LoginAccessDate { get; set; }
        public bool? IsPasswordLocked { get; set; }
        public int? LoginAttempt { get; set; }
        public Fellowship Fellowship { get; set; }
    }
}