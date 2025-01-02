using AttendanceSystem.Application.Responses;

namespace AttendanceSystem.Application.Features.Reports.Outreach.Queries.GetSingle
{
    public class GetOutreachReportQueryResponse : BaseResponse
    {
        public GetOutreachReportQueryResponse() : base() { }
        public OutreachReportDetailResultVM Result { get; set; } = default!;
    }
}