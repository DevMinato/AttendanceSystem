using AttendanceSystem.Application.Responses;
using MediatR;

namespace AttendanceSystem.Application.Features.Reports.Outreach.Commands.Create
{
    public class CreateOutreachReportCommand : IRequest<BaseResponse>
    {
        public Guid? MemberId { get; set; }
        public Guid? ActivityId { get; set; }
        public int TotalPeopleReached { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public List<CreateOutreachDetailCommand> OutreachDetails { get; set; } = new();
    }
}