using AttendanceSystem.Application.Models;

namespace AttendanceSystem.Application.Contracts.Infrastructure
{
    public interface IAttendanceStatisticsRepository
    {
        Task<List<AttendanceStatisticsViewModel>> GetAttendanceStatisticsAsync(DateTime startDate, DateTime endDate, Guid? fellowshipId, Guid? activityId, Guid? memberId);
    }
}
