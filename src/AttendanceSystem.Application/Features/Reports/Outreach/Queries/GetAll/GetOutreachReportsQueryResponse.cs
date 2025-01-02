using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;

namespace AttendanceSystem.Application.Features.Reports.Outreach.Queries.GetAll
{
    public class GetOutreachReportsQueryResponse : BaseResponse
    {
        public GetOutreachReportsQueryResponse() : base() { }
        public PagedResult<OutreachReportListResultVM> Result { get; set; } = default!;
    }
}