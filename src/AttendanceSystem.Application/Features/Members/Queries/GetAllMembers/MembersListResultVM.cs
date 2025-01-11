namespace AttendanceSystem.Application.Features.Members.Queries.GetAllMembers
{
    public class MembersListResultVM
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Guid? DisciplerId { get; set; }
        public string DisciplerFullName { get; set; }
        public Guid FellowshipId { get; set; }
        public string FellowshipName { get; set; }
        public string Status { get; set; }
    }
}