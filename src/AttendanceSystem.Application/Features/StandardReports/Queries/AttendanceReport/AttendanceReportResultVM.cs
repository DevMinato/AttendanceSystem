namespace AttendanceSystem.Application.Features.StandardReports.Queries.AttendanceReport
{
    public class AttendanceReportResultVM
    {
        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }
        public string MemberName { get; set; }
        public string ActivityName { get; set; }
        public int Attendance { get; set; }
    }
}