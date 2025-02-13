using AttendanceSystem.Application.Responses;
using MediatR;

namespace AttendanceSystem.Application.Features.Reports.Followup.Commands.Create
{
    public class CreateFollowupReportCommand : IRequest<BaseResponse>
    {
        public List<CreateFollowUpDetailCommand> FollowUpDetails { get; set; } = new();
    }
}