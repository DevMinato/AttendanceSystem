namespace AttendanceSystem.Application.Models
{
    public class AttendanceReportViewModel
    {
        //public DateTime Date { get; set; }
        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }
        //public Guid MemberId { get; set; }
        public string MemberName { get; set; }
        public string ActivityName { get; set; }
        public int Attendance { get; set; }
        //public string? Notes { get; set; }
        //public string? FirstTimer { get; set; }
    }
}