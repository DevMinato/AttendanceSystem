using MediatR;

namespace AttendanceSystem.Application.Features.Reports.Followup.Queries.GetSingle
{
    public class GetFollowupReportQuery : IRequest<GetFollowupReportQueryResponse>
    {
        public Guid? ReportId { get; set; }
    }
}