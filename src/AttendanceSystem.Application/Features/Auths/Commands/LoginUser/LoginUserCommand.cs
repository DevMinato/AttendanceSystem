using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Enums;
using MediatR;

namespace AttendanceSystem.Application.Features.Auths.Commands.LoginUser
{
    public class LoginUserCommand : IRequest<LoginUserCommandResponse>
    {
        public MemberType? MemberType { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
