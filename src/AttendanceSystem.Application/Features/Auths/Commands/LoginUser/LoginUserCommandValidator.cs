using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Domain.Entities;
using FluentValidation;

namespace AttendanceSystem.Application.Features.Auths.Commands.LoginUser
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        private readonly IAsyncRepository<Member> _memberRepository;
        private readonly IAsyncRepository<Pastor> _pastorRepository;


        public LoginUserCommandValidator(IAsyncRepository<Member> memberRepository, IAsyncRepository<Pastor> pastorRepository)
        {
            _memberRepository = memberRepository;
            _pastorRepository = pastorRepository;

            RuleFor(x => x.Email).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Email required")
                .NotNull()
                .WithMessage("Email required");

            RuleFor(x => x.Password).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Password is required")
                .NotNull()
                .WithMessage("Password is required");

            RuleFor(x => x)
                .MustAsync(UserExists)
                .WithMessage("Invalid credentials");
        }

        private async Task<bool> UserExists(LoginUserCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (command.MemberType == Domain.Enums.MemberType.WorkersInTraining)
                {
                    var user = await _memberRepository.GetSingleAsync(x => x.Email == command.Email);
                    if (user != null)
                        return true;
                }
                else if (command.MemberType == Domain.Enums.MemberType.Pastor)
                {
                    var user = await _pastorRepository.GetSingleAsync(x => x.Email == command.Email);
                    if (user != null)
                        return true;
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