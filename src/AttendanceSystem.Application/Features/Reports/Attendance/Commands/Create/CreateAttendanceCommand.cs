using AttendanceSystem.Application.Responses;
using MediatR;

namespace AttendanceSystem.Application.Features.Reports.Attendance.Commands.Create
{
    public class CreateAttendanceCommand : IRequest<BaseResponse>
    {
        public List<AttendanceCommand> Attendances { get; set; } = new List<AttendanceCommand>();
    }

    public class AttendanceCommand
    {
        public Guid? MemberId { get; set; }
        public Guid? ActivityId { get; set; }
        public bool IsPresent { get; set; } = false;
        public string? Notes { get; set; } = string.Empty;
        public bool? IsFirstTimer { get; set; } = false;
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}