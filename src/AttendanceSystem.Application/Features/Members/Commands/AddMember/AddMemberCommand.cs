using AttendanceSystem.Domain.Enums;
using MediatR;

namespace AttendanceSystem.Application.Features.Members.Commands.AddMember
{
    public class AddMemberCommand : IRequest<AddMemberCommandResponse>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public GenderEnum? Gender { get; set; }
        public MemberType? MemberType { get; set; }
        public Guid? DisciplerId { get; set; }
        public Guid? FellowshipId { get; set; }
    }
}