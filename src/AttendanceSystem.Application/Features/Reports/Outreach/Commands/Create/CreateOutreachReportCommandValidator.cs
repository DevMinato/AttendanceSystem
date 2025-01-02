using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Domain.Entities;
using FluentValidation;

namespace AttendanceSystem.Application.Features.Reports.Outreach.Commands.Create
{
    public class CreateOutreachReportCommandValidator : AbstractValidator<CreateOutreachReportCommand>
    {
        private readonly IAsyncRepository<Member> _memberRepository;
        private readonly IAsyncRepository<Activity> _activityRepository;
        public CreateOutreachReportCommandValidator(IAsyncRepository<Member> memberRepository, IAsyncRepository<Activity> activityRepository)
        {
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

            RuleFor(x => x.Date).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Activity date is required");
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