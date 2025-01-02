using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Domain.Entities;
using FluentValidation;

namespace AttendanceSystem.Application.Features.Members.Commands.DeleteMember
{
    public class DeleteMemberCommandValidator : AbstractValidator<DeleteMemberCommand>
    {
        private readonly IAsyncRepository<Member> _memberRepository;
        public DeleteMemberCommandValidator(IAsyncRepository<Member> memberRepository)
        {
            _memberRepository = memberRepository;
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
    }
}