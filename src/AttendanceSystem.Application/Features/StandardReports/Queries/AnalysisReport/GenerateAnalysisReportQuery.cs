using AttendanceSystem.Domain.Enums;
using MediatR;

namespace AttendanceSystem.Application.Features.StandardReports.Queries.AnalysisReport
{
    public class GenerateAnalysisReportQuery : IRequest<GenerateAnalysisReportQueryResponse>
    {
        public string? ActivityIds { get; set; }
        public Guid? FellowshipId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public PeriodType Period { get; set; } = PeriodType.Monthly;
    }
}