namespace AttendanceSystem.Application.Models
{
    public class PerformanceAnalysisViewModel
    {
        public string Activity { get; set; }
        public int TotalCount { get; set; }
        public int AttendanceCount { get; set; }
        public int OutreachCount { get; set; }
        public int FollowUpCount { get; set; }
    }
}