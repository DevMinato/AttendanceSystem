using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Domain.Entities;
using FluentValidation;

namespace AttendanceSystem.Application.Features.Reports.Followup.Commands.Delete
{
    public class DeleteFollowupReportCommandValidator : AbstractValidator<DeleteFollowupReportCommand>
    {
        private readonly IAsyncRepository<FollowUpReport> _followupReportRepository;
        public DeleteFollowupReportCommandValidator(IAsyncRepository<FollowUpReport> followupReportRepository)
        {
            _followupReportRepository = followupReportRepository;

            RuleFor(x => x.ReportId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Report identifier is required")
                .Must(x => BeValidReportId(x.Value).Result)
                .WithMessage("Report identifier is not valid.");
        }

        private async Task<bool> BeValidReportId(Guid id)
        {
            var count = await _followupReportRepository.CountAsync(x => x.Id == id);
            if (count == 0)
                return false;
            return true;
        }
    }
}