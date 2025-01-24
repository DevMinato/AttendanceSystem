using AttendanceSystem.Domain.Entities;
using MediatR;

namespace AttendanceSystem.Application.Features.Setups.StudyGroup.Queries.GetAllStudyGroups
{
    public class GetAllStudyGroupsQuery : PagingModel, IRequest<GetAllStudyGroupsQueryResponse>
    {
        public string? Search { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}