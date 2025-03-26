using Amazon.Runtime.Internal;
using MediatR;

namespace AttendanceSystem.Application.Features.Analytics.Queries.PerformanceAnalysis
{
    public class GetPerformanceAnalysisQuery : IRequest<GetPerformanceAnalysisQueryResponse>
    {
        public string? ActivityIds { get; set; }
        public Guid? MemberId { get; set; }
        public Guid? FellowshipId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}