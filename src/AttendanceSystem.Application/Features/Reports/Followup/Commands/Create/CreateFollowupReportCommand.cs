using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Enums;
using MediatR;

namespace AttendanceSystem.Application.Features.Reports.Followup.Commands.Create
{
    public class CreateFollowupReportCommand : IRequest<BaseResponse>
    {
        public Guid? MemberId { get; set; }
        public Guid? ActivityId { get; set; }
        public FollowUpType FollowUpType { get; set; }
        public int TotalFollowUps { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime? Date { get; set; } = DateTime.UtcNow;
        public List<CreateFollowUpDetailCommand> FollowUpDetails { get; set; } = new();
    }
}