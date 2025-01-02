using FluentValidation;

namespace AttendanceSystem.Application.Features.Auths.Commands.ApproveUser
{
    public class ApproverUserCommandValidator : AbstractValidator<ApproverUserCommand>
    {
        public ApproverUserCommandValidator()
        {

            RuleFor(x => x.UserId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("User identifier is required.");

            RuleFor(x => x.MemberType).Cascade(CascadeMode.Stop)
                .IsInEnum()
                .WithMessage("Invalid user type.");

            RuleFor(x => x.Status).Cascade(CascadeMode.Stop)
                .IsInEnum()
                .WithMessage("Invalid approval status.");
        }
    }
}
