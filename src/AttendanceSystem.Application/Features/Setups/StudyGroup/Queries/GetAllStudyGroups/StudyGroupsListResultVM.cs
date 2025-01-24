namespace AttendanceSystem.Application.Features.Setups.StudyGroup.Queries.GetAllStudyGroups
{
    public class StudyGroupsListResultVM
    {
        public Guid Id { get; set; }
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