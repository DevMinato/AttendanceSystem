using AttendanceSystem.Application.Contracts.Persistence;
using FluentValidation;

namespace AttendanceSystem.Application.Features.Setups.StudyGroup.Commands.Delete
{
    public class DeleteStudyGroupCommandValidator : AbstractValidator<DeleteStudyGroupCommand>
    {
        private readonly IAsyncRepository<Domain.Entities.StudyGroup> _studyGroupRepository;
        public DeleteStudyGroupCommandValidator(IAsyncRepository<Domain.Entities.StudyGroup> studyGroupRepository)
        {
            _studyGroupRepository = studyGroupRepository;

            RuleFor(x => x.StudyGroupId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Study group identifier is required")
                .Must(x => BeValidMemberId(x.Value).Result)
                .WithMessage("Study group identifier is not valid.");
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