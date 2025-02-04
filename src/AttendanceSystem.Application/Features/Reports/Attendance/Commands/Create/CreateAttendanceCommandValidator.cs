﻿using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Features.StudyGroup.Commands.Add;
using AttendanceSystem.Domain.Entities;
using FluentValidation;

namespace AttendanceSystem.Application.Features.Reports.Attendance.Commands.Create
{
    public class CreateAttendanceCommandValidator : AbstractValidator<CreateAttendanceCommand>
    {
        private readonly IAsyncRepository<Member> _memberRepository;
        private readonly IAsyncRepository<Activity> _activityRepository;
        private readonly IAsyncRepository<AttendanceReport> _attendanceReportRepository;
        public CreateAttendanceCommandValidator(IAsyncRepository<Member> memberRepository, IAsyncRepository<Activity> activityRepository, IAsyncRepository<AttendanceReport> attendanceReportRepository)
        {
            _memberRepository = memberRepository;
            _activityRepository = activityRepository;
            _attendanceReportRepository = attendanceReportRepository;

            RuleFor(x => x.MemberId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Member identifier is required")
                .Must(x => BeValidMemberId(x.Value).Result)
                .WithMessage("Member identifier is not valid.");

            RuleFor(x => x.ActivityId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithMessage("Activity identifier is required")
                .Must(x => BeValidActivity(x.Value).Result)
                .WithMessage("Activity identifier is not valid.");

            RuleFor(x => x)
                .MustAsync(HasMarkedAttendance)
                .WithMessage("Member has marked attendance.");
        }

        private async Task<bool> BeValidMemberId(Guid id)
        {
            var count = await _memberRepository.CountAsync(x => x.Id == id);
            if (count == 0)
                return false;
            return true;
        }

        private async Task<bool> BeValidActivity(Guid id)
        {
            var count = await _activityRepository.CountAsync(x => x.Id == id);
            if (count == 0)
                return false;
            return true;
        }

        private async Task<bool> HasMarkedAttendance(CreateAttendanceCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var attendance = await _attendanceReportRepository.GetSingleAsync(x => x.MemberId == command.MemberId && x.CreatedAt.Date == DateTime.UtcNow.Date);
                if (attendance == null)
                {
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