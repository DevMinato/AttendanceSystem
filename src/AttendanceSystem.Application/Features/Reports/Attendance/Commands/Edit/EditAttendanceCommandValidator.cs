using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Domain.Entities;
using FluentValidation;

namespace AttendanceSystem.Application.Features.Reports.Attendance.Commands.Edit
{
    public class EditAttendanceCommandValidator : AbstractValidator<EditAttendanceCommand>
    {
        private readonly IAsyncRepository<AttendanceReport> _attendanceReportRepository;
        private readonly IAsyncRepository<Member> _memberRepository;
        private readonly IAsyncRepository<Activity> _activityRepository;
        public EditAttendanceCommandValidator(IAsyncRepository<AttendanceReport> attendanceReportRepository, IAsyncRepository<Member> memberRepository, IAsyncRepository<Activity> activityRepository)
        {
            _attendanceReportRepository = attendanceReportRepository;

            _memberRepository = memberRepository;
            _activityRepository = activityRepository;

            RuleFor(x => x.MemberId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Member identifier is required")
                .Must(x => BeValidMemberId(x.Value).Result)
                .WithMessage("Member identifier is not valid.");

            RuleFor(x => x.ActivityId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Activity identifier is required")
                .Must(x => BeValidActivity(x.Value).Result)
                .WithMessage("Activity identifier is not valid.");

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

        private async Task<bool> BeValidMemberId(Guid id)
        {
            var count = await _memberRepository.CountAsync(x => x.Id == id);
            if (count == 0)
                return false;
            return true;
        }

        private async Task<bool> BeValidActivity(Guid id)
        {
            var count = await _activityRepository.CountAsync(x => x.Id == id);
            if (count == 0)
                return false;
            return true;
        }
    }
}