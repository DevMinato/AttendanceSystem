using AttendanceSystem.Application.Models;

namespace AttendanceSystem.Application.Contracts.Infrastructure
{
    public interface IAnalyticsRepository
    {
        Task<List<AttendanceStatisticsViewModel>> GetAttendanceStatisticsAsync(DateTime startDate, DateTime endDate, Guid? fellowshipId, 
            List<Guid>? activityIds, Guid? memberId);

        Task<List<PerformanceAnalysisViewModel>> GetPerformanceAnalysisAsync(DateTime startDate, DateTime endDate, Guid? fellowshipId,
            List<Guid>? activityIds, Guid? memberId);
    }
}
