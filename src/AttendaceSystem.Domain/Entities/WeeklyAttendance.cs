using AttendanceSystem.Domain.Enums;

namespace AttendanceSystem.Domain.Entities
{
    public class WeeklyAttendance : BaseEntity
    {
        public WeekEnum Week { get; set; }
        public long MemberId { get; set; }
        public List<ActivityReport> ActivityReports { get; set; }
    }
}