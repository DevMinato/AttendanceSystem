﻿namespace AttendanceSystem.Application.Features.StandardReports.Queries.Exports.AttendanceReport
{
    public class AttendanceReportExportFileVm
    {
        public string ExportFileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public byte[]? Data { get; set; }
    }
}