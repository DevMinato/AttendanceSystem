namespace AttendanceSystem.Application.Features.Reports.Attendance.Queries.GetAll
{
    public class AttendanceReportListResultVM
    {
        public Guid Id { get; set; }
        public Guid MemberId { get; set; }
        public string MemberFullName { get; set; }
        public Guid ActivityId { get; set; }
        public string ActivityName { get; set; }
        public bool IsPresent { get; set; }
        public DateTime Date { get; set; }
    }
}