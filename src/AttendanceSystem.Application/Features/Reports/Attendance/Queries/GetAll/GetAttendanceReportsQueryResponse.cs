using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;

namespace AttendanceSystem.Application.Features.Reports.Attendance.Queries.GetAll
{
    public class GetAttendanceReportsQueryResponse : BaseResponse
    {
        public GetAttendanceReportsQueryResponse() : base() { }
        public PagedResult<AttendanceReportListResultVM> Result { get; set; } = default!;
    }
}