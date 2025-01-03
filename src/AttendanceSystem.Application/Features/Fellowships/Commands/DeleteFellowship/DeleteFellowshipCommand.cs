using AttendanceSystem.Application.Responses;
using MediatR;

namespace AttendanceSystem.Application.Features.Fellowships.Commands.DeleteFellowship
{
    public class DeleteFellowshipCommand : IRequest<BaseResponse>
    {
        public Guid? FellowshipId { get; set; }
    }
}