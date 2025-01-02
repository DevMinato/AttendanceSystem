using AttendanceSystem.Application.Responses;

namespace AttendanceSystem.Application.Features.Reports.Attendance.Queries.GetSingle
{
    public class GetAttendanceReportQueryResponse : BaseResponse
    {
        public GetAttendanceReportQueryResponse() : base() { }
        public AttendanceReportDetailResultVM Result { get; set; } = default!;
    }
}