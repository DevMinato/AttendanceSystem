using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Enums;
using MediatR;

namespace AttendanceSystem.Application.Features.Auths.Commands.ApproveUser
{
    public class ApproverUserCommand : IRequest<BaseResponse>
    {
        public MemberType? MemberType { get; set; }
        public Guid? UserId { get; set; }
        public ApprovalStatus? Status { get; set; }
    }
}