using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Enums;
using MediatR;

namespace AttendanceSystem.Application.Features.Auths.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<BaseResponse>
    {
        public MemberType? MemberType { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public Guid? FellowshipId { get; set; }
    }
}