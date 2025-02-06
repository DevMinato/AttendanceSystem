using AttendanceSystem.Application.Contracts.Infrastructure;
using AttendanceSystem.Application.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AttendanceSystem.Persistence.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly IConfiguration _configuration;
        string _connectionString = string.Empty;
        public ReportRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("AppConnectionString");
        }
        public async Task<List<AttendanceReportViewModel>> FetchAttendanceData(DateTime startDate, DateTime endDate, Guid? activityId)
        {
            string query = @"
                WITH DateRange AS (
                    -- Ensure @StartDate is cast to DATE to match recursive part
                    SELECT CAST(@StartDate AS DATE) AS Date
                    UNION ALL
                    SELECT DATEADD(DAY, 1, Date) 
                    FROM DateRange 
                    WHERE Date < @EndDate
                ),
                WeeklyGroups AS (
                    SELECT 
                        DATEADD(DAY, -(DATEPART(WEEKDAY, Date) - 2), Date) AS WeekStart,
                        DATEADD(DAY, 6 - (DATEPART(WEEKDAY, Date) - 2), Date) AS WeekEnd
                    FROM DateRange
                    GROUP BY 
                        DATEADD(DAY, -(DATEPART(WEEKDAY, Date) - 2), Date),
                        DATEADD(DAY, 6 - (DATEPART(WEEKDAY, Date) - 2), Date)  -- Include WeekEnd in GROUP BY
                )
                SELECT 
                    m.FirstName + ' ' + m.LastName AS MemberName,
                    w.WeekStart, 
                    w.WeekEnd,
                    a.Id AS ActivityId,
                    a.Name AS ActivityName,
                    COALESCE(ar.IsPresent, 0) AS Attendance
                FROM [wt-db].[RS].Members m
                CROSS JOIN WeeklyGroups w
                CROSS JOIN [wt-db].[RS].Activities a  -- Ensures all members are listed for all activities
                LEFT JOIN [wt-db].[RS].AttendanceReports ar 
                    ON ar.MemberId = m.Id 
                    AND ar.ActivityId = a.Id
                    AND ar.Date BETWEEN w.WeekStart AND w.WeekEnd
                WHERE (@ActivityId IS NULL OR a.Id = @ActivityId) -- Apply filter if activityId is provided
                ";


            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = startDate },
                new SqlParameter("@EndDate", SqlDbType.DateTime) { Value = endDate },
                new SqlParameter("@ActivityId", SqlDbType.UniqueIdentifier) { Value = (object?)activityId ?? DBNull.Value }
            };

            /*if (activityId.HasValue)
            {
                parameters.Add(new SqlParameter("@ActivityId", SqlDbType.UniqueIdentifier) { Value = activityId.Value });
            }*/

            query += @"            
            ORDER BY
               w.WeekStart, MemberName, a.Name
            OPTION (MAXRECURSION 0);";

            List<AttendanceReportViewModel> attendanceRecords = new List<AttendanceReportViewModel>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();


                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddRange(parameters.ToArray());
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            attendanceRecords.Add(new AttendanceReportViewModel
                            {
                                MemberName = reader["MemberName"].ToString(),
                                WeekStart = Convert.ToDateTime(reader["WeekStart"]),
                                WeekEnd = Convert.ToDateTime(reader["WeekEnd"]),
                                ActivityName = reader["ActivityName"].ToString(),
                                Attendance = Convert.ToInt32(reader["Attendance"])
                            });
                        }
                    }
                }
            }

            return attendanceRecords;
        }

        public async Task<List<MonthlyAttendanceReportViewModel>> FetchMonthlyAttendanceReportAsync(DateTime startDate, DateTime endDate, Guid? fellowshipId)
        {
            var query = @"
            WITH ActivityFrequency AS (
                SELECT 
                    ar.ActivityId, 
                    COUNT(DISTINCT ar.Date) AS Frequency
                FROM RS.AttendanceReports ar
                WHERE ar.Date BETWEEN @startDate AND @endDate
                GROUP BY ar.ActivityId
            ),
            MemberAttendance AS (
                SELECT 
                    ar.ActivityId,
                    ar.MemberId,
                    COUNT(DISTINCT ar.Date) AS AttendanceCount
                FROM RS.AttendanceReports ar
                JOIN RS.Members m ON ar.MemberId = m.Id
                WHERE ar.Date BETWEEN @startDate AND @endDate
                    AND (@fellowshipId IS NULL OR m.FellowshipId = @fellowshipId)
                GROUP BY ar.ActivityId, ar.MemberId
            )
            SELECT 
                a.Name AS Activity,
                af.Frequency AS Frequency,
                COUNT(DISTINCT ma.MemberId) AS TotalAttendees,
                COUNT(CASE WHEN ma.AttendanceCount = af.Frequency THEN 1 END) AS Count_100_Percent,
                COUNT(CASE WHEN ma.AttendanceCount >= 0.75 * af.Frequency AND ma.AttendanceCount < af.Frequency THEN 1 END) AS Count_75_Percent,
                COUNT(CASE WHEN ma.AttendanceCount >= 0.5 * af.Frequency AND ma.AttendanceCount < 0.75 * af.Frequency THEN 1 END) AS Count_50_Percent,
                COUNT(CASE WHEN ma.AttendanceCount < 0.5 * af.Frequency THEN 1 END) AS Count_Below_50_Percent,
                COUNT(DISTINCT m.DisciplerId) AS MembersWithDisciples
            FROM MemberAttendance ma
            JOIN ActivityFrequency af ON ma.ActivityId = af.ActivityId
            JOIN RS.Activities a ON ma.ActivityId = a.Id
            JOIN RS.Members m ON ma.MemberId = m.Id
            WHERE (@fellowshipId IS NULL OR m.FellowshipId = @fellowshipId)
            GROUP BY a.Name, af.Frequency
            ORDER BY a.Name;";

            var report = new List<MonthlyAttendanceReportViewModel>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", endDate);
                    command.Parameters.AddWithValue("@fellowshipId", (object?)fellowshipId ?? DBNull.Value);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            report.Add(new MonthlyAttendanceReportViewModel
                            {
                                Activity = reader.GetString(0),
                                Frequency = reader.GetInt32(1),
                                TotalAttendees = reader.GetInt32(2),
                                Count100 = reader.GetInt32(3),
                                Count75 = reader.GetInt32(4),
                                Count50 = reader.GetInt32(5),
                                CountBelow50 = reader.GetInt32(6),
                                MembersWithDisciples = reader.GetInt32(7)
                            });
                        }
                    }
                }
            }

            return report;
        }
    }
}