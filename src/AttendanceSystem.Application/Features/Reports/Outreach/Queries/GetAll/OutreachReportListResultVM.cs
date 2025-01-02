namespace AttendanceSystem.Application.Features.Reports.Outreach.Queries.GetAll
{
    public class OutreachReportListResultVM
    {
        public Guid Id { get; set; }
        public Guid MemberId { get; set; }
        public string MemberFullName { get; set; }
        public Guid ActivityId { get; set; }
        public string ActivityName { get; set; }
        public int TotalPeopleReached { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
        public List<OutreachDetailResultVM>? OutreachDetails { get; set; }
    }
}