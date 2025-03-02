using AttendanceSystem.Domain.Enums;
using MediatR;

namespace AttendanceSystem.Application.Features.Fellowships.Commands.AddFellowship
{
    public class AddFellowshipCommand : IRequest<AddFellowshipCommandResponse>
    {
        public string? Name { get; set; }
        public Guid? ParentId { get; set; }
        public Guid? PastorId { get; set; }
    }
}