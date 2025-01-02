using AttendanceSystem.Application.Responses;
using MediatR;

namespace AttendanceSystem.Application.Features.Reports.Followup.Commands.Delete
{
    public class DeleteFollowupReportCommand : IRequest<BaseResponse>
    {
        public Guid? ReportId { get; set; }
    }
}