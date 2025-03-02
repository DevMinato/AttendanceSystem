using AttendanceSystem.Domain.Enums;

namespace AttendanceSystem.Application.Features.Reports.Followup.Queries.GetSingle
{
    public class FollowupReportDetailResultVM
    {
        public Guid Id { get; set; }
        public string FollowUpType { get; set; }
        public Guid DiscipleId { get; set; }
        public string DiscipleFullName { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
    }
}