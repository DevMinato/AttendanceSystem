using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;

namespace AttendanceSystem.Application.Features.Reports.Followup.Queries.GetAll
{
    public class GetFollowupReportsQueryResponse : BaseResponse
    {
        public GetFollowupReportsQueryResponse() : base() { }
        public PagedResult<FollowupReportListResultVM> Result { get; set; } = default!;
    }
}