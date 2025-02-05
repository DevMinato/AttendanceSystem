using AttendanceSystem.Domain.Entities;
using MediatR;

namespace AttendanceSystem.Application.Features.Reports.Attendance.Queries.GetAll
{
    public class GetAttendanceReportsQuery : PagingModel, IRequest<GetAttendanceReportsQueryResponse>
    {
        public Guid? MemberId { get; set; }
        public Guid? ActivityId { get; set; }
        public string? Search { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}