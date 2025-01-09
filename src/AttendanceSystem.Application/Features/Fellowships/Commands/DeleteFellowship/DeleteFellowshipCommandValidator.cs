using AttendanceSystem.Application.Contracts.Persistence;
using FluentValidation;

namespace AttendanceSystem.Application.Features.Fellowships.Commands.DeleteFellowship
{
    public class DeleteFellowshipCommandValidator : AbstractValidator<DeleteFellowshipCommand>
    {
        private readonly IAsyncRepository<Domain.Entities.Fellowship> _fellowshipRepository;
        public DeleteFellowshipCommandValidator(IAsyncRepository<Domain.Entities.Fellowship> fellowshipRepository)
        {
            _fellowshipRepository = fellowshipRepository;
            RuleFor(x => x.FellowshipId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Fellowship identifier is required")
                .Must(x => BeValidMemberId(x.Value).Result)
                .WithMessage("Fellowship identifier is not valid.");
        }

        private async Task<bool> BeValidMemberId(Guid id)
        {
            var count = await _fellowshipRepository.CountAsync(x => x.Id == id);
            if (count == 0)
                return false;
            return true;
        }
    }
}