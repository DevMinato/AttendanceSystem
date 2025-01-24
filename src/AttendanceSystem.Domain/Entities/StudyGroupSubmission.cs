using Microsoft.AspNetCore.Http;

namespace AttendanceSystem.Domain.Entities
{
    public class StudyGroupSubmission : BaseEntity
    {
        public Guid StudyGroupId { get; set; }
        public Guid MemberId { get; set; }
        public IFormFile Upload { get; set; }
        public StudyGroup StudyGroup { get; set; }
    }
}