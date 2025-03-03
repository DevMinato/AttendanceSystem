﻿namespace AttendanceSystem.Application.Models
{
    public class AttendanceReportExportDto
    {
        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }
        public string MemberName { get; set; }
        public string ActivityName { get; set; }
        public int Attendance { get; set; }
        public int TotalPresent { get; set; }
        public int TotalSessions { get; set; }
        public decimal AttendancePercentage { get; set; }
    }
}