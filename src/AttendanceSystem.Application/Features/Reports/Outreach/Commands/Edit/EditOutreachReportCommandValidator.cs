using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Domain.Entities;
using FluentValidation;

namespace AttendanceSystem.Application.Features.Reports.Outreach.Commands.Edit
{
    public class EditOutreachReportCommandValidator : AbstractValidator<EditOutreachReportCommand>
    {
        private readonly IAsyncRepository<OutreachReport> _outreachReportRepository;
        private readonly IAsyncRepository<Member> _memberRepository;
        private readonly IAsyncRepository<Activity> _activityRepository;
        public EditOutreachReportCommandValidator(IAsyncRepository<OutreachReport> outreachReportRepository, IAsyncRepository<Member> memberRepository, IAsyncRepository<Activity> activityRepository)
        {
            _outreachReportRepository = outreachReportRepository;
            _memberRepository = memberRepository;
            _activityRepository = activityRepository;

            RuleFor(x => x.ReportId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Report identifier is required")
                .Must(x => BeValidReportId(x.Value).Result)
                .WithMessage("Report identifier is not valid.");

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

            RuleFor(x => x.Date).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Activity date is required");
        }

        private async Task<bool> BeValidReportId(Guid id)
        {
            var count = await _outreachReportRepository.CountAsync(x => x.Id == id);
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