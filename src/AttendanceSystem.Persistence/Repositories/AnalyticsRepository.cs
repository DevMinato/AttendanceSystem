using AttendanceSystem.Application.Contracts.Infrastructure;
using AttendanceSystem.Application.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AttendanceSystem.Persistence.Repositories
{
    public class AnalyticsRepository : IAnalyticsRepository
    {
        private readonly IConfiguration _configuration;
        string _connectionString = string.Empty;
        public AnalyticsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("AppConnectionString");
        }
        public async Task<List<AttendanceStatisticsViewModel>> GetAttendanceStatisticsAsync(DateTime startDate, DateTime endDate, Guid? fellowshipId, 
            List<Guid>? activityIds, Guid? memberId)
        {
            var query = @"
            SELECT 
                a.Name AS Activity, 
                COUNT(CASE WHEN ar.IsPresent = 1 THEN 1 END) AS TotalCount
            FROM RS.AttendanceReports ar
            JOIN RS.Activities a ON ar.ActivityId = a.Id
            WHERE ar.Date BETWEEN @startDate AND @endDate
                AND (@fellowshipId IS NULL OR ar.MemberId IN (SELECT Id FROM RS.Members WHERE FellowshipId = @fellowshipId))
                AND (@activityIds IS NULL OR ar.ActivityId IN (SELECT value FROM STRING_SPLIT(@activityIds, ',')))
                AND (@memberId IS NULL OR ar.MemberId = @memberId)
            GROUP BY a.Name
            ORDER BY a.Name;";

            var statistics = new List<AttendanceStatisticsViewModel>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", endDate);
                    command.Parameters.AddWithValue("@fellowshipId", (object?)fellowshipId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@activityIds", activityIds != null && activityIds.Any() ? string.Join(",", activityIds) : DBNull.Value);
                    command.Parameters.AddWithValue("@memberId", (object?)memberId ?? DBNull.Value);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            statistics.Add(new AttendanceStatisticsViewModel
                            {
                                Activity = reader.GetString(0),
                                TotalCount = reader.GetInt32(1).ToString()
                            });
                        }
                    }
                }
            }

            return statistics;
        }

        public async Task<List<PerformanceAnalysisViewModel>> GetPerformanceAnalysisAsync(DateTime startDate, DateTime endDate, Guid? fellowshipId, List<Guid>? activityIds, Guid? memberId)
        {
            var query = @"
            SELECT 
                a.Name AS Activity, 
                COALESCE(SUM(attendance.TotalCount), 0) AS AttendanceCount,
                COALESCE(SUM(outreach.TotalPeopleReached), 0) AS OutreachCount,
                COALESCE(SUM(followup.TotalFollowUps), 0) AS FollowUpCount
            FROM RS.Activities a
            LEFT JOIN (
                -- Attendance Report
                SELECT 
                    ar.ActivityId, 
                    COUNT(*) AS TotalCount
                FROM RS.AttendanceReports ar
                JOIN RS.Members m ON ar.MemberId = m.Id
                WHERE ar.IsPresent = 1
                    AND ar.CreatedAt BETWEEN @startDate AND @endDate
                    AND (@fellowshipId IS NULL OR m.FellowshipId = @fellowshipId)
                    AND (@memberId IS NULL OR ar.MemberId = @memberId)
                GROUP BY ar.ActivityId
            ) attendance ON a.Id = attendance.ActivityId
            LEFT JOIN (
                -- Outreach Report (using OutreachDetails for accurate count)
                SELECT 
                    o.ActivityId, 
                    COUNT(od.Id) AS TotalPeopleReached
                FROM RS.OutreachReports o
                JOIN RS.OutreachDetails od ON o.Id = od.OutreachReportId
                JOIN RS.Members m ON o.MemberId = m.Id
                WHERE o.CreatedAt BETWEEN @startDate AND @endDate
                    AND (@fellowshipId IS NULL OR m.FellowshipId = @fellowshipId)
                    AND (@memberId IS NULL OR o.MemberId = @memberId)
                GROUP BY o.ActivityId
            ) outreach ON a.Id = outreach.ActivityId
            LEFT JOIN (
                -- Follow-up Report (using FollowUpDetails for accurate count)
                SELECT 
                    f.ActivityId, 
                    COUNT(fd.Id) AS TotalFollowUps
                FROM RS.FollowUpReports f
                JOIN RS.FollowUpDetails fd ON f.Id = fd.FollowUpReportId
                JOIN RS.Members m ON f.MemberId = m.Id
                WHERE f.CreatedAt BETWEEN @startDate AND @endDate
                    AND (@fellowshipId IS NULL OR m.FellowshipId = @fellowshipId)
                    AND (@memberId IS NULL OR f.MemberId = @memberId)
                GROUP BY f.ActivityId
            ) followup ON a.Id = followup.ActivityId
            WHERE (@activityIds IS NULL OR a.Id IN (
                SELECT CAST(value AS uniqueidentifier) FROM STRING_SPLIT(@activityIds, ',')
            ))
            GROUP BY a.Name
            ORDER BY a.Name;";

            var statistics = new List<PerformanceAnalysisViewModel>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", endDate);
                    command.Parameters.AddWithValue("@fellowshipId", (object?)fellowshipId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@activityIds", activityIds != null && activityIds.Any() ? string.Join(",", activityIds) : DBNull.Value);
                    command.Parameters.AddWithValue("@memberId", (object?)memberId ?? DBNull.Value);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            statistics.Add(new PerformanceAnalysisViewModel
                            {
                                Activity = reader.GetString(0),
                                AttendanceCount = reader.GetInt32(1),
                                OutreachCount = reader.GetInt32(2),
                                FollowUpCount = reader.GetInt32(3),
                                TotalCount = reader.GetInt32(1) + reader.GetInt32(2) + reader.GetInt32(3)
                            });
                        }
                    }
                }
            }

            return statistics;
        }
    }
}