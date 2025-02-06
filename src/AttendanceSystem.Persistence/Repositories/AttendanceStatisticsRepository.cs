using AttendanceSystem.Application.Contracts.Infrastructure;
using AttendanceSystem.Application.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AttendanceSystem.Persistence.Repositories
{
    public class AttendanceStatisticsRepository : IAttendanceStatisticsRepository
    {
        private readonly IConfiguration _configuration;
        string _connectionString = string.Empty;
        public AttendanceStatisticsRepository(IConfiguration configuration)
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

    }
}