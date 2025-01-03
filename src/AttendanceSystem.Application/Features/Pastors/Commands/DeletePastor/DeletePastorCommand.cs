using AttendanceSystem.Application.Responses;
using MediatR;

namespace AttendanceSystem.Application.Features.Pastors.Commands.DeletePastor
{
    public class DeletePastorCommand : IRequest<BaseResponse>
    {
        public Guid? PastorId { get; set; }
    }
}