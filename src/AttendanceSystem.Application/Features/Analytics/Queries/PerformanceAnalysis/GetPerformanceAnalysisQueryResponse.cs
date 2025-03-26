using AttendanceSystem.Application.Models;
using AttendanceSystem.Application.Responses;

namespace AttendanceSystem.Application.Features.Analytics.Queries.PerformanceAnalysis
{
    public class GetPerformanceAnalysisQueryResponse : BaseResponse
    {
        public GetPerformanceAnalysisQueryResponse()
        {
        }

        public List<PerformanceAnalysisViewModel> Result { get; set; }
    }
}
