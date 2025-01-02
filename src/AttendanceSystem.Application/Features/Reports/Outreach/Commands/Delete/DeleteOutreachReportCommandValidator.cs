using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Domain.Entities;
using FluentValidation;

namespace AttendanceSystem.Application.Features.Reports.Outreach.Commands.Delete
{
    public class DeleteOutreachReportCommandValidator : AbstractValidator<DeleteOutreachReportCommand>
    {
        private readonly IAsyncRepository<OutreachReport> _outreachReportRepository;
        public DeleteOutreachReportCommandValidator(IAsyncRepository<OutreachReport> outreachReportRepository)
        {
            _outreachReportRepository = outreachReportRepository;

            RuleFor(x => x.ReportId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Report identifier is required")
                .Must(x => BeValidReportId(x.Value).Result)
                .WithMessage("Report identifier is not valid.");
        }

        private async Task<bool> BeValidReportId(Guid id)
        {
            var count = await _outreachReportRepository.CountAsync(x => x.Id == id);
            if (count == 0)
                return false;
            return true;
        }
    }
}