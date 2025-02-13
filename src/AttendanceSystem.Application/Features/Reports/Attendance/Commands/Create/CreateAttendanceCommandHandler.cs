using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Reports.Attendance.Commands.Create
{
    public class CreateAttendanceCommandHandler : IRequestHandler<CreateAttendanceCommand, BaseResponse>
    {
        private readonly IAsyncRepository<AttendanceReport> _attendanceReportRepository;
        private readonly ILogger<CreateAttendanceCommandHandler> _logger;
        private readonly IAsyncRepository<Member> _memberRepository;
        private readonly IAsyncRepository<Activity> _activityRepository;
        private readonly IMapper _mapper;
        public CreateAttendanceCommandHandler(IAsyncRepository<AttendanceReport> attendanceReportRepository, ILogger<CreateAttendanceCommandHandler> logger,
            IAsyncRepository<Member> memberRepository, IAsyncRepository<Activity> activityRepository, IMapper mapper)
        {
            _attendanceReportRepository = attendanceReportRepository;
            _logger = logger;
            _memberRepository = memberRepository;
            _activityRepository = activityRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse> Handle(CreateAttendanceCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();
            try
            {
                var validator = new CreateAttendanceCommandValidator(_memberRepository, _activityRepository, _attendanceReportRepository);
                var validationResult = await validator.ValidateAsync(request);
                if (validationResult.Errors.Count > 0)
                    throw new ValidationException(validationResult);

                List<AttendanceReport> reports = new List<AttendanceReport>();

                foreach (var attendance in request.Attendances)
                {
                    var report = _mapper.Map<AttendanceReport>(attendance);
                    reports.Add(report);
                }

                await _attendanceReportRepository.AddRangeAsync(reports);

                response.Success = true;
                response.Message = Constants.SuccessResponse;
            }
            catch (CustomException ex)
            {
                response.Message = ex.Message;
            }
            catch (ValidationException ex)
            {
                response.ValidationErrors = ex.ValidationErrors;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response.Success = false;
                response.Message = Constants.ErrorResponse;
            }

            return response;
        }
    }
}