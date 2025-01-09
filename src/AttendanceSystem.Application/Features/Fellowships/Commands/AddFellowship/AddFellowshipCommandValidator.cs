using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Features.Auths.Commands.CreateUser;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Enums;
using FluentValidation;

namespace AttendanceSystem.Application.Features.Fellowships.Commands.AddFellowship
{
    public class AddFellowshipCommandValidator : AbstractValidator<AddFellowshipCommand>
    {
        private readonly IAsyncRepository<Fellowship> _fellowshipRepository;
        public AddFellowshipCommandValidator(IAsyncRepository<Fellowship> fellowshipRepository)
        {
            _fellowshipRepository = fellowshipRepository;
            RuleFor(x => x.Name).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Fellowship name is required")
                .MaximumLength(20)
                .WithMessage("Fellowship name cannot exceed 20 characters");

            RuleFor(x => x.PastorId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Pastor identifier is required");

            RuleFor(x => x)
                .MustAsync(IsUnique)
                .WithMessage("Fellowship name already exist");
        }

        private bool BeValidGenderType(GenderEnum? gender)
        {
            return Enum.IsDefined(typeof(GenderEnum), gender);
        }

        private async Task<bool> IsUnique(AddFellowshipCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var fellowship = await _fellowshipRepository.GetSingleAsync(x => x.Name == command.Name);
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
