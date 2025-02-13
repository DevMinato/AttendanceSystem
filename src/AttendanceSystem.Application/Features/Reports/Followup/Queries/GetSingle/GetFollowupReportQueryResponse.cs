using AttendanceSystem.Application.Responses;

namespace AttendanceSystem.Application.Features.Reports.Followup.Queries.GetSingle
{
    public class GetFollowupReportQueryResponse : BaseResponse
    {
        public GetFollowupReportQueryResponse() : base() { }
        public List<FollowupReportDetailResultVM> Result { get; set; } = default!;
    }
}