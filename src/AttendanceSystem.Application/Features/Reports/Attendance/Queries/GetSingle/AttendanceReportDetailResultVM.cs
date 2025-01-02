namespace AttendanceSystem.Application.Features.Reports.Attendance.Queries.GetSingle
{
    public class AttendanceReportDetailResultVM
    {
        public Guid MemberId { get; set; }
        public string MemberFullName { get; set; }
        public Guid ActivityId { get; set; }
        public string ActivityName { get; set; }
        public bool IsPresent { get; set; }
        public DateTime Date { get; set; }

        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
