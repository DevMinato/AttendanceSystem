using AttendanceSystem.Application.Models;
using System.Data;

namespace AttendanceSystem.Application.Contracts.Infrastructure
{
    public interface IExcelExporter
    {
        byte[] ExportAttendanceReportsToExcel(List<AttendanceReportExportDto> reportExportDtos);
    }
}