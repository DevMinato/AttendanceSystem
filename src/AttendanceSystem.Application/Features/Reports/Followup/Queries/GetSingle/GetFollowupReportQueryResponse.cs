using AttendanceSystem.Application.Features.Reports.Attendance.Queries.GetAll;
using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;

namespace AttendanceSystem.Application.Features.Reports.Followup.Queries.GetSingle
{
    public class GetFollowupReportQueryResponse : BaseResponse
    {
        public GetFollowupReportQueryResponse() : base() { }
        public FollowupReportDetailResultVM Result { get; set; } = default!;
    }
}