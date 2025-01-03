using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Domain.Entities;
using FluentValidation;

namespace AttendanceSystem.Application.Features.Fellowships.Commands.EditFellowship
{
    public class EditFellowshipCommandValidator : AbstractValidator<EditFellowshipCommand>
    {
        private readonly IAsyncRepository<Fellowship> _fellowshipRepository;
        public EditFellowshipCommandValidator(IAsyncRepository<Fellowship> fellowshipRepository)
        {
            _fellowshipRepository = fellowshipRepository;

            RuleFor(x => x.FellowshipId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Fellowship identifier is required")
                .Must(x => BeValidFellowshipId(x.Value).Result)
                .WithMessage("Fellowship identifier is not valid.");
        }

        private async Task<bool> BeValidFellowshipId(Guid id)
        {
            var count = await _fellowshipRepository.CountAsync(x => x.Id == id);
            if (count == 0)
                return false;
            return true;
        }
    }
}