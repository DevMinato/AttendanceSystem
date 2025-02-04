namespace AttendanceSystem.Application.Features.StudyGroup.Queries.GetSingle
{
    public class StudyGroupSubmissionDetailResultVM
    {
        public Guid StudyGroupId { get; set; }
        public Guid MemberId { get; set; }
        public string MemberFullName { get; set; }
        public StudyGroupResultVM? StudyGroup { get; set; }

        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class StudyGroupResultVM
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Week { get; set; }
        public string StudyGroupMaterial { get; set; }
        public string StudyGroupQuestion { get; set; }
        public DateTime DeadlineDate { get; set; }
        public bool AllowLateSubmission { get; set; } = false;
    }
}