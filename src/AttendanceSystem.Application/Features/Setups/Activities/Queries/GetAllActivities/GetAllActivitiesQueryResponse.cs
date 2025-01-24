using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;

namespace AttendanceSystem.Application.Features.Setups.Activities.Queries.GetAllActivities
{
    public class GetAllActivitiesQueryResponse : BaseResponse
    {
        public GetAllActivitiesQueryResponse() : base() { }
        public PagedResult<ActivitiesListResultVM> Result { get; set; } = default!;
    }
}