using AttendanceSystem.Domain.Enums;

namespace AttendanceSystem.Application.Features.Reports.Followup.Commands.Create
{
    public class CreateFollowUpDetailCommand
    {
        public Guid? MemberId { get; set; }
        public Guid? ActivityId { get; set; }
        public FollowUpType FollowUpType { get; set; }
        public Guid? DiscipleId { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime? Date { get; set; } = DateTime.UtcNow;
    }
}