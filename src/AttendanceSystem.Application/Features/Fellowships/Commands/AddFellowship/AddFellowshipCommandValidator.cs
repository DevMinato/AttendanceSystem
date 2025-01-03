using AttendanceSystem.Domain.Enums;
using FluentValidation;

namespace AttendanceSystem.Application.Features.Fellowships.Commands.AddFellowship
{
    public class AddFellowshipCommandValidator : AbstractValidator<AddFellowshipCommand>
    {
        public AddFellowshipCommandValidator()
        {
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

        }

        private bool BeValidGenderType(GenderEnum? gender)
        {
            return Enum.IsDefined(typeof(GenderEnum), gender);
        }
    }
}
