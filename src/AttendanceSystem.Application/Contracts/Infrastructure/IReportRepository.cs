using AttendanceSystem.Application.Models;

namespace AttendanceSystem.Application.Contracts.Infrastructure
{
    public interface IReportRepository
    {
        Task<List<AttendanceReportViewModel>> FetchAttendanceData(DateTime startDate, DateTime endDate, List<Guid>? activityIds);
        Task<List<MonthlyAttendanceReportViewModel>> FetchMonthlyAttendanceReportAsync(DateTime startDate, DateTime endDate, Guid? fellowshipId, List<Guid>? activityIds);
    }
}