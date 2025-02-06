using MediatR;

namespace AttendanceSystem.Application.Features.StandardReports.Queries.AttendanceReport
{
    public class GenerateAttendanceReportQuery : IRequest<GenerateAttendanceReportQueryResponse>
    {
        public string? ActivityIds { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}