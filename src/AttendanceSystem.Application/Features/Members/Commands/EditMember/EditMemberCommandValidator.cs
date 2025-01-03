using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Domain.Entities;
using FluentValidation;

namespace AttendanceSystem.Application.Features.Members.Commands.EditMember
{
    public class EditMemberCommandValidator : AbstractValidator<EditMemberCommand>
    {
        private readonly IAsyncRepository<Member> _memberRepository;
        private readonly IAsyncRepository<Fellowship> _fellowshipRepository;
        public EditMemberCommandValidator(IAsyncRepository<Member> memberRepository, IAsyncRepository<Fellowship> fellowshipRepository)
        {
            _memberRepository = memberRepository;
            _fellowshipRepository = fellowshipRepository;

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

            RuleFor(x => x.PhoneNumber).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Phone number is required")
                .MaximumLength(15)
                .WithMessage("Phone number cannot exceed 15 characters")
                .MinimumLength(11)
                .WithMessage("Phone number cannot be less than 11 characters");


            RuleFor(x => x.Email).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Email is required")
                .NotNull()
                .WithMessage("Email is required")
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$")
                .WithMessage("Invalid email format")
                .EmailAddress()
                .WithMessage("invalid email format");

            RuleFor(x => x.FellowshipId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Fellowship identifier is required")
                .Must(x => BeValidFellowship(x).Result)
                .WithMessage("Fellowship identifier is not valid.");

            RuleFor(x => x.Gender).Cascade(CascadeMode.Stop)
                .IsInEnum()
                .WithMessage("Invalid gender selected");

            RuleFor(x => x)
                .MustAsync(IsEmailAddressUnique)
                .WithMessage("Email address exists")
                .MustAsync(IsPhoneNumberUnique)
                .WithMessage("Phone number exists");

            RuleFor(x => x.MemberId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Member identifier is required")
                .Must(x => BeValidMemberId(x.Value).Result)
                .WithMessage("Member identifier is not valid.");
        }

        private async Task<bool> BeValidMemberId(Guid id)
        {
            var count = await _memberRepository.CountAsync(x => x.Id == id);
            if (count == 0)
                return false;
            return true;
        }

        private async Task<bool> IsEmailAddressUnique(EditMemberCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _memberRepository.GetSingleAsync(x => x.Email == command.Email && x.Id != command.MemberId);
                if (user == null)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }

        async Task<bool> IsPhoneNumberUnique(EditMemberCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _memberRepository.GetSingleAsync(x => x.PhoneNumber == command.PhoneNumber && x.Id != command.MemberId);
                if (user == null)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }

        }

        private async Task<bool> BeValidFellowship(Guid? id)
        {
            var count = await _fellowshipRepository.CountAsync(x => x.Id == id);
            if (count == 0)
                return false;
            return true;
        }
    }
}