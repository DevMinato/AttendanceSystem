using AttendanceSystem.Application.Models;
using System.Data;

namespace AttendanceSystem.Application.Contracts.Infrastructure
{
    public interface IReportRepository
    {
        Task<List<AttendanceReportViewModel>> FetchAttendanceData(DateTime startDate, DateTime endDate, Guid? activityId);
    }
}