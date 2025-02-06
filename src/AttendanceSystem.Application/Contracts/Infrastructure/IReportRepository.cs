using AttendanceSystem.Application.Models;
using System.Data;
using System.Threading.Tasks;

namespace AttendanceSystem.Application.Contracts.Infrastructure
{
    public interface IReportRepository
    {
        Task<List<AttendanceReportViewModel>> FetchAttendanceData(DateTime startDate, DateTime endDate, Guid? activityId);
        Task<List<MonthlyAttendanceReportViewModel>> FetchMonthlyAttendanceReportAsync(DateTime startDate, DateTime endDate, Guid? fellowshipId);
    }
}