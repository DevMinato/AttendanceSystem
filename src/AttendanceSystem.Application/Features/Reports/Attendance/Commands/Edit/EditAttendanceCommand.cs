using AttendanceSystem.Application.Responses;
using MediatR;

namespace AttendanceSystem.Application.Features.Reports.Attendance.Commands.Edit
{
    public class EditAttendanceCommand : IRequest<BaseResponse>
    {
        public Guid? ReportId { get; set; }
        public Guid? MemberId { get; set; }
        public Guid? ActivityId { get; set; }
        public bool IsPresent { get; set; } = false;
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}