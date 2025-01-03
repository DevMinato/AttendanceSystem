namespace AttendanceSystem.Application.Features.Auths.Commands.LoginUser
{
    public class UserResponse
    {
        public Guid UserId { get; set; }
        public string EmailAddress { get; set; }
        public string? MobileNumber { get; set; }
        public string? FullName { get; set; }
        public string UserType { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string GroupName { get; set; }
        public Guid? GroupId { get; set; }
        public List<string>? Permissions { get; set; }
    }
}