using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Enums;
using MediatR;

namespace AttendanceSystem.Application.Features.Members.Queries.GetAllMembers
{
    public class GetAllMembersQuery : PagingModel, IRequest<GetAllMembersQueryResponse>
    {
        public string? Search { get; set; }
        public Guid? FellowshipId { get; set; }
        public Guid? DisciplerId { get; set; }
        public GenderEnum? Gender { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}