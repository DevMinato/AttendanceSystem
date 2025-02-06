using AttendanceSystem.Application.Models;
using AttendanceSystem.Domain.Enums;

namespace AttendanceSystem.Application.Contracts.Infrastructure
{
    public interface IWordExporter
    {
        Task<byte[]> ExportAttendanceReportToWordAsync(DateTime startDate, DateTime endDate, string fellowshipName, string period, List<MonthlyAttendanceReportViewModel> reportData);
    }
}