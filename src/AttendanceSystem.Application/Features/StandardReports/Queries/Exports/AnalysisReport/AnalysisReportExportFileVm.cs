namespace AttendanceSystem.Application.Features.StandardReports.Queries.Exports.AnalysisReport
{
    public class AnalysisReportExportFileVm
    {
        public string ExportFileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public byte[]? Data { get; set; }
    }
}