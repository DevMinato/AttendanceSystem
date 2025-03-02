using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Domain.Entities;
using FluentValidation;

namespace AttendanceSystem.Application.Features.Reports.Followup.Commands.Create
{
    public class CreateFollowupReportCommandValidator : AbstractValidator<CreateFollowupReportCommand>
    {
        private readonly IAsyncRepository<Member> _memberRepository;
        private readonly IAsyncRepository<Activity> _activityRepository;

        public CreateFollowupReportCommandValidator(IAsyncRepository<Member> memberRepository, IAsyncRepository<Activity> activityRepository)
        {
            _memberRepository = memberRepository;
            _activityRepository = activityRepository;

            RuleFor(x => x.FollowUpDetails)
                .NotEmpty()
                .WithMessage("Follow-up details cannot be empty.")
                .Must(HaveConsistentMemberAndActivityIds)
                .WithMessage("All Follow-up details must have the same MemberId and ActivityId.");

            RuleForEach(x => x.FollowUpDetails).SetValidator(new CreateFollowUpDetailCommandValidator(_memberRepository, _activityRepository));
        }

        private bool HaveConsistentMemberAndActivityIds(List<CreateFollowUpDetailCommand> followUpDetails)
        {
            if (followUpDetails == null || !followUpDetails.Any()) return true;

            var firstMemberId = followUpDetails.First().MemberId;
            var firstActivityId = followUpDetails.First().ActivityId;

            return followUpDetails.All(x => x.MemberId == firstMemberId && x.ActivityId == firstActivityId);
        }
    }

    public class CreateFollowUpDetailCommandValidator : AbstractValidator<CreateFollowUpDetailCommand>
    {
        private readonly IAsyncRepository<Member> _memberRepository;
        private readonly IAsyncRepository<Activity> _activityRepository;

        public CreateFollowUpDetailCommandValidator(IAsyncRepository<Member> memberRepository, IAsyncRepository<Activity> activityRepository)
        {
            _memberRepository = memberRepository;
            _activityRepository = activityRepository;

            RuleFor(x => x.MemberId)
                .NotEmpty()
                .NotNull()
                .WithMessage("Member identifier is required.")
                .MustAsync(async (id, _) => await BeValidMemberId(id.Value))
                .WithMessage("Member identifier is not valid.");

            RuleFor(x => x.DiscipleId)
               .NotEmpty()
               .NotNull()
               .WithMessage("Disciple identifier is required.")
               .MustAsync(async (id, _) => await BeValidMemberId(id.Value))
               .WithMessage("Disciple identifier is not valid.");

            RuleFor(x => x.ActivityId)
                .NotEmpty()
                .NotNull()
                .WithMessage("Activity identifier is required.")
                .MustAsync(async (id, _) => await BeValidActivity(id.Value))
                .WithMessage("Activity identifier is not valid.");

            RuleFor(x => x.FollowUpType)
                .IsInEnum()
                .WithMessage("Invalid follow-up type.");

            RuleFor(x => x.Date)
                .NotEmpty()
                .NotNull()
                .WithMessage("Activity date is required.");
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