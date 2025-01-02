using AttendanceSystem.Application.Responses;
using MediatR;

namespace AttendanceSystem.Application.Features.Reports.Attendance.Commands.Delete
{
    public class DeleteAttendanceCommand : IRequest<BaseResponse>
    {
        public Guid? ReportId { get; set; }
    }
}