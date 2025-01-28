using AttendanceSystem.Application.Contracts.Persistence;
using FluentValidation;

namespace AttendanceSystem.Application.Features.StudyGroup.Commands.Edit
{
    public class EditStudyGroupSubmissionCommandValidator : AbstractValidator<EditStudyGroupSubmissionCommand>
    {
        private readonly IAsyncRepository<Domain.Entities.StudyGroup> _studyGroupRepository;
        private readonly IAsyncRepository<Domain.Entities.StudyGroupSubmission> _studyGroupSubmissionRepository;
        public EditStudyGroupSubmissionCommandValidator(IAsyncRepository<Domain.Entities.StudyGroup> studyGroupRepository, IAsyncRepository<Domain.Entities.StudyGroupSubmission> studyGroupSubmissionRepository)
        {
            _studyGroupRepository = studyGroupRepository;
            _studyGroupSubmissionRepository = studyGroupSubmissionRepository;

            RuleFor(x => x.SubmissionId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Study group submission identifier is required")
                .Must(x => IsValid(x).Result)
                .WithMessage("Study group question identifier is not valid.");

            RuleFor(x => x.StudyGroupId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Study group question identifier is required")
                .Must(x => BeValidStudyGroupId(x).Result)
                .WithMessage("Study group question identifier is not valid.");

            RuleFor(x => x.MemberId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Member identifier is required");

            RuleFor(x => x.Upload).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Kindly upload your answer.");

            RuleFor(x => x)
                .MustAsync(HasDeadlinePassed)
                .WithMessage("Deadline date for submission has passsed.");
        }

        private async Task<bool> IsValid(Guid? id)
        {
            try
            {
                return await _studyGroupSubmissionRepository.CountAsync(x => x.Id == id) > 0;
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

        private async Task<bool> HasDeadlinePassed(EditStudyGroupSubmissionCommand command, CancellationToken cancellationToken)
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
    }
}
