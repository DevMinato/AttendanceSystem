using AttendanceSystem.Application.Contracts.Persistence;
using FluentValidation;

namespace AttendanceSystem.Application.Features.Setups.StudyGroup.Commands.Add
{
    public class AddStudyGroupCommandValidator : AbstractValidator<AddStudyGroupCommand>
    {
        private readonly IAsyncRepository<Domain.Entities.StudyGroup> _studyGroupRepository;
        public AddStudyGroupCommandValidator(IAsyncRepository<Domain.Entities.StudyGroup> studyGroupRepository)
        {
            _studyGroupRepository = studyGroupRepository;

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
                .WithMessage("Study group for this period already exist");
        }

        private async Task<bool> IsUnique(AddStudyGroupCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var fellowship = await _studyGroupRepository.GetSingleAsync(x => x.From >= command.From && x.To <= command.To);
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
