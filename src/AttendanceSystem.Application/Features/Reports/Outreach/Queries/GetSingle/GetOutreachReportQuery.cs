using MediatR;

namespace AttendanceSystem.Application.Features.Reports.Outreach.Queries.GetSingle
{
    public class GetOutreachReportQuery : IRequest<GetOutreachReportQueryResponse>
    {
        public Guid? ReportId { get; set; }
    }
}