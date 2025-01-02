using AttendanceSystem.Application.Responses;
using MediatR;

namespace AttendanceSystem.Application.Features.Reports.Outreach.Commands.Delete
{
    public class DeleteOutreachReportCommand : IRequest<BaseResponse>
    {
        public Guid? ReportId { get; set; }
    }
}