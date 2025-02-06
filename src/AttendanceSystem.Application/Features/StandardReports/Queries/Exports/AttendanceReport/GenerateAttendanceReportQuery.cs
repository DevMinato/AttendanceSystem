using MediatR;

namespace AttendanceSystem.Application.Features.StandardReports.Queries.Exports.AttendanceReport
{
    public class GenerateAttendanceReportQuery : IRequest<AttendanceReportExportFileVm>
    {
        public string ExportType { get; set; } = "excel";
        public Guid? ActivityId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}