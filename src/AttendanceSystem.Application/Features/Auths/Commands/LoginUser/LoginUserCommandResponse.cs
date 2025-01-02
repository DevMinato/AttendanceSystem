using AttendanceSystem.Application.Models;
using AttendanceSystem.Application.Responses;

namespace AttendanceSystem.Application.Features.Auths.Commands.LoginUser
{
    public class LoginUserCommandResponse : BaseResponse
    {
        public LoginUserCommandResponse() : base()
        {

        }

        public JwtAuthResult Result { get; set; } = default!;
    }
}