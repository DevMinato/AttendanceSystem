using AttendanceSystem.Application.Features.Members.Queries.GetAllMembers;
using AttendanceSystem.Application.Responses;

namespace AttendanceSystem.Application.Features.Members.Commands.AddMember
{
    public class AddMemberCommandResponse : BaseResponse
    {
        public AddMemberCommandResponse() : base() { }
        public MembersListResultVM Result { get; set; } = default!;
    }
}