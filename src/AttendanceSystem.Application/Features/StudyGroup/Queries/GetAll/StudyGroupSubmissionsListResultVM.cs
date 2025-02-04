using AttendanceSystem.Application.Features.StudyGroup.Queries.GetSingle;

namespace AttendanceSystem.Application.Features.StudyGroup.Queries.GetAll
{
    public class StudyGroupSubmissionsListResultVM
    {
        public Guid Id { get; set; }
        public Guid StudyGroupId { get; set; }
        public Guid MemberId { get; set; }
        public string MemberFullName { get; set; }
        public StudyGroupResultVM? StudyGroup { get; set; }
    }
}