namespace AttendanceSystem.Application.Features.StandardReports.Queries.AnalysisReport
{
    public class AnalysisReportResultVM
    {
        public string Activity { get; set; }
        public int Frequency { get; set; } = 0;
        public int TotalAttendees { get; set; } = 0;
        public int Count100 { get; set; } = 0;
        public int Count75 { get; set; } = 0;
        public int Count50 { get; set; } = 0;
        public int CountBelow50 { get; set; } = 0;
        public int MembersWithDisciples { get; set; } = 0;
    }
}