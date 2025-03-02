using AttendanceSystem.Domain.Enums;

namespace AttendanceSystem.Application.Features.Reports.Followup.Queries.GetAll
{
    public class FollowUpDetailResultVM
    {
        public Guid MemberId { get; set; }
        public string MemberFullName { get; set; }
        public FollowUpType FollowUpType { get; set; }
        public Guid DiscipleId { get; set; }
        public string DiscipleFullName { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime? Date { get; set; }
    }
}