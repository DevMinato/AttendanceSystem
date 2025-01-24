using AttendanceSystem.Domain.Entities;
using MediatR;

namespace AttendanceSystem.Application.Features.Setups.Activities.Queries.GetAllActivities
{
    public class GetAllActivitiesQuery : PagingModel, IRequest<GetAllActivitiesQueryResponse>
    {
        public string? Search { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}