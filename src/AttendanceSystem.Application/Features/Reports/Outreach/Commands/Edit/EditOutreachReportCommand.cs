using AttendanceSystem.Application.Responses;
using MediatR;

namespace AttendanceSystem.Application.Features.Reports.Outreach.Commands.Edit
{
    public class EditOutreachReportCommand : IRequest<BaseResponse>
    {
        public Guid? ReportId { get; set; }
        public Guid? MemberId { get; set; }
        public Guid? ActivityId { get; set; }
        public int TotalPeopleReached { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public List<EditOutreachDetailCommand> OutreachDetails { get; set; } = new();
    }
}