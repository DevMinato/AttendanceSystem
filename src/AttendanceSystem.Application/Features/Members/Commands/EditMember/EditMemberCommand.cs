using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Enums;
using MediatR;

namespace AttendanceSystem.Application.Features.Members.Commands.EditMember
{
    public class EditMemberCommand : IRequest<BaseResponse>
    {
        public Guid? MemberId { get; set; }
        public Guid? FellowshipId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public GenderEnum? Gender { get; set; }
        public MemberType? MemberType { get; set; }
        public Guid? DisciplerId { get; set; }
        public bool IsActive { get; set; }
    }
}