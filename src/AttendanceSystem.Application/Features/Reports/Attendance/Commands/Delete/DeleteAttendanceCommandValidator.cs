using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Domain.Entities;
using FluentValidation;

namespace AttendanceSystem.Application.Features.Reports.Attendance.Commands.Delete
{
    public class DeleteAttendanceCommandValidator : AbstractValidator<DeleteAttendanceCommand>
    {
        private readonly IAsyncRepository<AttendanceReport> _attendanceReportRepository;
        public DeleteAttendanceCommandValidator(IAsyncRepository<AttendanceReport> attendanceReportRepository)
        {
            _attendanceReportRepository = attendanceReportRepository;

            RuleFor(x => x.ReportId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Report identifier is required")
                .Must(x => BeValidReportId(x.Value).Result)
                .WithMessage("Report identifier is not valid.");
        }

        private async Task<bool> BeValidReportId(Guid id)
        {
            var count = await _attendanceReportRepository.CountAsync(x => x.Id == id);
            if (count == 0)
                return false;
            return true;
        }
    }
}