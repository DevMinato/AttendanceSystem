using AttendanceSystem.Domain.Enums;

namespace AttendanceSystem.Application.Features.Reports.Followup.Queries.GetSingle
{
    public class FollowupReportDetailResultVM
    {
        public string FollowUpType { get; set; }
        public string FullName { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
    }
}