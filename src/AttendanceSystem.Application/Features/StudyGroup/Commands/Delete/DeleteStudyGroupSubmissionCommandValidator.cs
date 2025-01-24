using AttendanceSystem.Application.Contracts.Persistence;
using FluentValidation;

namespace AttendanceSystem.Application.Features.StudyGroup.Commands.Delete
{
    internal class DeleteStudyGroupSubmissionCommandValidator : AbstractValidator<DeleteStudyGroupSubmissionCommand>
    {
        private readonly IAsyncRepository<Domain.Entities.StudyGroupSubmission> _studyGroupRepository;
        public DeleteStudyGroupSubmissionCommandValidator(IAsyncRepository<Domain.Entities.StudyGroupSubmission> studyGroupRepository)
        {
            _studyGroupRepository = studyGroupRepository;

            RuleFor(x => x.StudyGroupSubmissionId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Study group submission identifier is required")
                .Must(x => BeValidMemberId(x.Value).Result)
                .WithMessage("Study group submission identifier is not valid.");
        }

        private async Task<bool> BeValidMemberId(Guid id)
        {
            var count = await _studyGroupRepository.CountAsync(x => x.Id == id);
            if (count == 0)
                return false;
            return true;
        }
    }
}