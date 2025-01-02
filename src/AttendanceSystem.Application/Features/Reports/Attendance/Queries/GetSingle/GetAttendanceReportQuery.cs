using MediatR;

namespace AttendanceSystem.Application.Features.Reports.Attendance.Queries.GetSingle
{
    public class GetAttendanceReportQuery : IRequest<GetAttendanceReportQueryResponse>
    {
        public Guid? ReportId { get; set; }
    }
}