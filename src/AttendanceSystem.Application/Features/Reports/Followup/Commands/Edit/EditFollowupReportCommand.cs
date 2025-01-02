using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Enums;
using MediatR;

namespace AttendanceSystem.Application.Features.Reports.Followup.Commands.Edit
{
    public class EditFollowupReportCommand : IRequest<BaseResponse>
    {
        public Guid? ReportId { get; set; }
        public Guid? MemberId { get; set; }
        public Guid? ActivityId { get; set; }
        public FollowUpType FollowUpType { get; set; }
        public int TotalFollowUps { get; set; }
        public string Notes { get; set; }
        public DateTime? Date { get; set; }
        public List<EditFollowUpDetailCommand> FollowUpDetails { get; set; } = new();
    }
}