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
    }
}