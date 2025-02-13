using AttendanceSystem.Domain.Entities;
using MediatR;

namespace AttendanceSystem.Application.Features.Reports.Outreach.Queries.GetAll
{
    public class GetOutreachReportsQuery : PagingModel, IRequest<GetOutreachReportsQueryResponse>
    {
        public string? Search { get; set; }
        public Guid? MemberId { get; set; }
        public Guid? DisciplerId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}