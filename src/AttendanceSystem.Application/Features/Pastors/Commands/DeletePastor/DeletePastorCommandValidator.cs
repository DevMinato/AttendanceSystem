using AttendanceSystem.Application.Contracts.Persistence;
using FluentValidation;

namespace AttendanceSystem.Application.Features.Pastors.Commands.DeletePastor
{
    public class DeletePastorCommandValidator : AbstractValidator<DeletePastorCommand>
    {
        private readonly IAsyncRepository<Domain.Entities.Pastor> _pastorRepository;
        public DeletePastorCommandValidator(IAsyncRepository<Domain.Entities.Pastor> pastorRepository)
        {
            _pastorRepository = pastorRepository;
            RuleFor(x => x.PastorId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Pastor identifier is required")
                .Must(x => BeValidPastorId(x.Value).Result)
                .WithMessage("Pastor identifier is not valid.");
        }

        private async Task<bool> BeValidPastorId(Guid id)
        {
            var count = await _pastorRepository.CountAsync(x => x.Id == id);
            if (count == 0)
                return false;
            return true;
        }
    }
}