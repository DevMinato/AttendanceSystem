using AttendanceSystem.Application.Responses;

namespace AttendanceSystem.Application.Features.StandardReports.Queries.AttendanceReport
{
    public class GenerateAttendanceReportQueryResponse : BaseResponse
    {
        public GenerateAttendanceReportQueryResponse() : base() { }
        public List<AttendanceReportResultVM> Result { get; set; } = default!;
    }
}