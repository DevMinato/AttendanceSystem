using AttendanceSystem.Application.Contracts.Persistence;
using FluentValidation;

namespace AttendanceSystem.Application.Features.StudyGroup.Commands.Add
{
    public class AddStudyGroupSubmissionCommandValidator : AbstractValidator<AddStudyGroupSubmissionCommand>
    {
        private readonly IAsyncRepository<Domain.Entities.StudyGroup> _studyGroupRepository;
        private readonly IAsyncRepository<Domain.Entities.StudyGroupSubmission> _studyGroupSubmissionRepository;
        public AddStudyGroupSubmissionCommandValidator(IAsyncRepository<Domain.Entities.StudyGroup> studyGroupRepository, IAsyncRepository<Domain.Entities.StudyGroupSubmission> studyGroupSubmissionRepository)
        {
            _studyGroupRepository = studyGroupRepository;
            _studyGroupSubmissionRepository = studyGroupSubmissionRepository;

            RuleFor(x => x.StudyGroupId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Study group identifier is required")
                .Must(x => BeValidStudyGroupId(x).Result)
                .WithMessage("Study group identifier is not valid.");

            RuleFor(x => x.MemberId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Member identifier is required");

            RuleFor(x => x.Upload).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Kindly upload your answer.");

            RuleFor(x => x)
                .MustAsync(IsUnique)
                .WithMessage("Study group for this member already exist.")
                .MustAsync(HasDeadlinePassed)
                .WithMessage("Deadline date for submission has passsed.");
        }

        private async Task<bool> IsUnique(AddStudyGroupSubmissionCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var fellowship = await _studyGroupSubmissionRepository.GetSingleAsync(x => x.MemberId == command.MemberId && x.StudyGroupId == command.StudyGroupId);
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

        private async Task<bool> HasDeadlinePassed(AddStudyGroupSubmissionCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var studyGroup = await _studyGroupRepository.GetSingleAsync(x => x.Id == command.StudyGroupId && x.AllowLateSubmission == false);
                if (studyGroup != null)
                {
                    if (DateTime.UtcNow > studyGroup.DeadlineDate) return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }

        private async Task<bool> BeValidStudyGroupId(Guid? id)
        {
            var count = await _studyGroupRepository.CountAsync(x => x.Id == id);
            if (count == 0)
                return false;
            return true;
        }
    }
}
