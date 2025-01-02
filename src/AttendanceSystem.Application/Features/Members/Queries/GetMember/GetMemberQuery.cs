using MediatR;

namespace AttendanceSystem.Application.Features.Members.Queries.GetMember
{
    public class GetMemberQuery : IRequest<GetMemberQueryResponse>
    {
        public Guid Id { get; set; }
    }
}