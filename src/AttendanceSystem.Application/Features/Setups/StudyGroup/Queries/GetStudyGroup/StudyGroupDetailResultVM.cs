namespace AttendanceSystem.Application.Features.Setups.StudyGroup.Queries.GetStudyGroup
{
    public class StudyGroupDetailResultVM
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

        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}