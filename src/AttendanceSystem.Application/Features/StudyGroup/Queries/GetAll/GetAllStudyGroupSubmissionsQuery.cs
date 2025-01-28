using AttendanceSystem.Domain.Entities;
using MediatR;

namespace AttendanceSystem.Application.Features.StudyGroup.Queries.GetAll
{
    public class GetAllStudyGroupSubmissionsQuery : PagingModel, IRequest<GetAllStudyGroupSubmissionsQueryResponse>
    {
        public string? Search { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}