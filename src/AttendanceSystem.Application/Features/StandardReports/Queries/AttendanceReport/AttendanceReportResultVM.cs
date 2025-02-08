namespace AttendanceSystem.Application.Features.StandardReports.Queries.AttendanceReport
{
    /*public class AttendanceReportResultVM
    {
        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }
        public string MemberName { get; set; }
        public string ActivityName { get; set; }
        public int Attendance { get; set; }
    }*/

    public class ActivityData
    {
        public Dictionary<string, List<ActivityEntry>> Members { get; set; } = new();
    }

    public class ActivityEntry
    {
        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }
        public string ActivityName { get; set; }
        public int Attendance { get; set; }
    }
}