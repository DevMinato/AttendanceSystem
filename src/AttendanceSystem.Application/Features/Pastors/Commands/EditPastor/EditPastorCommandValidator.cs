using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Features.Auths.Commands.CreateUser;
using AttendanceSystem.Domain.Entities;
using FluentValidation;

namespace AttendanceSystem.Application.Features.Pastors.Commands.EditPastor
{
    public class EditPastorCommandValidator : AbstractValidator<EditPastorCommand>
    {
        private readonly IAsyncRepository<Pastor> _pastorRepository;
        private readonly IAsyncRepository<Fellowship> _fellowshipRepository;
        public EditPastorCommandValidator(IAsyncRepository<Pastor> pastorRepository, IAsyncRepository<Fellowship> fellowshipRepository)
        {
            _pastorRepository = pastorRepository;
            _fellowshipRepository = fellowshipRepository;

            RuleFor(x => x.PastorId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Pastor identifier is required")
                .Must(x => BeValidPastorId(x.Value).Result)
                .WithMessage("Pastor identifier is not valid.");

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
        }

        private async Task<bool> IsEmailAddressUnique(EditPastorCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _pastorRepository.GetSingleAsync(x => x.Email == command.Email && x.Id != command.PastorId);
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

        async Task<bool> IsPhoneNumberUnique(EditPastorCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _pastorRepository.GetSingleAsync(x => x.PhoneNumber == command.PhoneNumber && x.Id != command.PastorId);
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

        private async Task<bool> BeValidPastorId(Guid id)
        {
            var count = await _pastorRepository.CountAsync(x => x.Id == id);
            if (count == 0)
                return false;
            return true;
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