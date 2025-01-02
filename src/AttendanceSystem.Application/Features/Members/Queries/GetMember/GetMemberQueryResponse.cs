using AttendanceSystem.Application.Responses;

namespace AttendanceSystem.Application.Features.Members.Queries.GetMember
{
    public class GetMemberQueryResponse : BaseResponse
    {
        public GetMemberQueryResponse() : base() { }
        public MemberDetailResultVM Result { get; set; } = default!;
    }
}