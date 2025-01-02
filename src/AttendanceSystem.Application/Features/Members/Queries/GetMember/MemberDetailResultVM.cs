namespace AttendanceSystem.Application.Features.Members.Queries.GetMember
{
    public class MemberDetailResultVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Guid? DisciplerId { get; set; }
        public string DisciplerFullName { get; set; }
        public Guid FellowshipId { get; set; }
        public string FellowshipName { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LoginAccessDate { get; set; }
        public bool? IsPasswordLocked { get; set; }
        public int? LoginAttempt { get; set; }

        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}