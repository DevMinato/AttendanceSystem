using MediatR;

namespace AttendanceSystem.Application.Features.Analytics.Queries.AttendanceStatistics
{
    public class GetAttendanceStatisticsQuery : IRequest<GetAttendanceStatisticsQueryResponse>
    {
        public string? ActivityIds { get; set; }
        public Guid? MemberId { get; set; }
        public Guid? FellowshipId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
