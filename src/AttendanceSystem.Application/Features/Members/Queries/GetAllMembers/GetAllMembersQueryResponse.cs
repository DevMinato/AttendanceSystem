using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;

namespace AttendanceSystem.Application.Features.Members.Queries.GetAllMembers
{
    public class GetAllMembersQueryResponse : BaseResponse
    {
        public GetAllMembersQueryResponse() : base() { }
        public PagedResult<MembersListResultVM> Result { get; set; } = default!;
    }
}