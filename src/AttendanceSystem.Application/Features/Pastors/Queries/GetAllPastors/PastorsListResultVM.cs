namespace AttendanceSystem.Application.Features.Pastors.Queries.GetAllPastors
{
    public class PastorsListResultVM
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public string Gender { get; set; }
        public Guid FellowshipId { get; set; }
        public string FellowshipName { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LoginAccessDate { get; set; }
        public bool? IsPasswordLocked { get; set; }
        public int? LoginAttempt { get; set; }
    }
}