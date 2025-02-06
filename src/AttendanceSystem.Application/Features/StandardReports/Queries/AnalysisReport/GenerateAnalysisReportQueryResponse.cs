using AttendanceSystem.Application.Responses;

namespace AttendanceSystem.Application.Features.StandardReports.Queries.AnalysisReport
{
    public class GenerateAnalysisReportQueryResponse : BaseResponse
    {
        public GenerateAnalysisReportQueryResponse() : base() { }
        public List<AnalysisReportResultVM> Result { get; set; } = default!;
    }
}