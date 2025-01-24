using AttendanceSystem.Application.Contracts.Persistence;
using FluentValidation;

namespace AttendanceSystem.Application.Features.Setups.StudyGroup.Commands.Edit
{
    public class EditStudyGroupCommandValidator : AbstractValidator<EditStudyGroupCommand>
    {
        private readonly IAsyncRepository<Domain.Entities.StudyGroup> _studyGroupRepository;
        public EditStudyGroupCommandValidator(IAsyncRepository<Domain.Entities.StudyGroup> studyGroupRepository)
        {
            _studyGroupRepository = studyGroupRepository;

            RuleFor(x => x.StudyGroupId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Study group identifier is required")
                .Must(x => BeValidFellowshipId(x.Value).Result)
                .WithMessage("Study group is not valid.");

            RuleFor(x => x.From).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Start date is required");

            RuleFor(x => x.To).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("End date is required");

            RuleFor(x => x.StudyGroupMaterial).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Study group material is required");

            RuleFor(x => x.StudyGroupQuestion).Cascade(CascadeMode.Stop)
               .NotEmpty()
               .NotNull()
               .WithMessage("Study group question is required");

            RuleFor(x => x.DeadlineDate).Cascade(CascadeMode.Stop)
               .NotEmpty()
               .NotNull()
               .WithMessage("Submission deadline date is required");

            RuleFor(x => x)
                .MustAsync(IsUnique)
                .WithMessage("Fellowship name already exist");
        }

        private async Task<bool> BeValidFellowshipId(Guid id)
        {
            var count = await _studyGroupRepository.CountAsync(x => x.Id == id);
            if (count == 0)
                return false;
            return true;
        }
        private async Task<bool> IsUnique(EditStudyGroupCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var fellowship = await _studyGroupRepository.GetSingleAsync(x => x.From >= command.From && x.To <= command.To && x.Id != command.StudyGroupId);
                if (fellowship == null)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }
    }
}