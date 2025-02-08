namespace AttendanceSystem.Application.Models
{
    public class AttendanceReportViewModel
    {
        public string MemberName { get; set; }  // Full name of the member
        public DateTime WeekStart { get; set; } // Start date of the week
        public DateTime WeekEnd { get; set; }   // End date of the week
        public Guid ActivityId { get; set; }    // Unique identifier of the activity
        public string ActivityName { get; set; } // Activity title (e.g., Sunday Service, Outreach)
        public int Attendance { get; set; }      // 1 = Present, 0 = Absent
        public int TotalPresent { get; set; }    // Number of times attended this activity in the month
        public int TotalSessions { get; set; }   // Total number of times the activity was held in the month
        public decimal AttendancePercentage { get; set; }
    }
}