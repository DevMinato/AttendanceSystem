using AttendanceSystem.Domain.Enums;
using FluentValidation;

namespace AttendanceSystem.Application.Features.Members.Commands.AddMember
{
    public class AddMemberCommandValidator : AbstractValidator<AddMemberCommand>
    {
        public AddMemberCommandValidator()
        {
            RuleFor(x => x.FirstName).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("First name is required")
                .MaximumLength(20)
                .WithMessage("First name cannot exceed 20 characters");

            RuleFor(x => x.LastName).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Last name is required")
                .MaximumLength(20)
                .WithMessage("Last name cannot exceed 20 characters");

            RuleFor(x => x.Gender).Cascade(CascadeMode.Stop)
                .Must(BeValidGenderType)
                .WithMessage("Selected employer type is not valid.");


            RuleFor(x => x.MemberType).Cascade(CascadeMode.Stop)
                .IsInEnum()
                .WithMessage("Invalid member type.");
        }

        private bool BeValidGenderType(GenderEnum? gender)
        {
            return Enum.IsDefined(typeof(GenderEnum), gender);
        }
    }
}
