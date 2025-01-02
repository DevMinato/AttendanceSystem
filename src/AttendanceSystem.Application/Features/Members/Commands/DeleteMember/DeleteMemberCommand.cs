using AttendanceSystem.Application.Responses;
using MediatR;

namespace AttendanceSystem.Application.Features.Members.Commands.DeleteMember
{
    public class DeleteMemberCommand : IRequest<BaseResponse>
    {
        public Guid? MemberId { get; set; }
    }
}