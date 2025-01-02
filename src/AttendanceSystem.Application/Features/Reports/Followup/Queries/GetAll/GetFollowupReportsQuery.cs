using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Enums;
using MediatR;

namespace AttendanceSystem.Application.Features.Reports.Followup.Queries.GetAll
{
    public class GetFollowupReportsQuery : PagingModel, IRequest<GetFollowupReportsQueryResponse>
    {
        public string? Search { get; set; }
        public Guid? MemberId { get; set; }
        public FollowUpType? FollowUpType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}