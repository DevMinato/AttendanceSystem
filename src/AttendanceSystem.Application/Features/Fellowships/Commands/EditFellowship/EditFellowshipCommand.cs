using AttendanceSystem.Application.Responses;
using MediatR;

namespace AttendanceSystem.Application.Features.Fellowships.Commands.EditFellowship
{
    public class EditFellowshipCommand : IRequest<BaseResponse>
    {
        public Guid? FellowshipId { get; set; }
        public string? Name { get; set; }
        public Guid? PastorId { get; set; }
        public bool IsActive { get; set; }
    }
}