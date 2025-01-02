using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Reports.Attendance.Commands.Edit
{
    public class EditAttendanceCommandHandler : IRequestHandler<EditAttendanceCommand, BaseResponse>
    {
        private readonly ILogger<EditAttendanceCommandHandler> _logger;
        private readonly IAsyncRepository<AttendanceReport> _attendanceReportRepository;
        private readonly IAsyncRepository<Member> _memberRepository;
        private readonly IAsyncRepository<Activity> _activityRepository;
        private readonly IMapper _mapper;
        public EditAttendanceCommandHandler(ILogger<EditAttendanceCommandHandler> logger, IAsyncRepository<AttendanceReport> attendanceReportRepository, 
            IMapper mapper, IAsyncRepository<Member> memberRepository, IAsyncRepository<Activity> activityRepository)
        {
            _logger = logger;
            _attendanceReportRepository = attendanceReportRepository;
            _mapper = mapper;
            _memberRepository = memberRepository;
            _activityRepository = activityRepository;
        }

        public async Task<BaseResponse> Handle(EditAttendanceCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();
            try
            {
                var validator = new EditAttendanceCommandValidator(_attendanceReportRepository, _memberRepository, _activityRepository);
                var validationResult = await validator.ValidateAsync(request);
                if (validationResult.Errors.Count > 0)
                    throw new ValidationException(validationResult);

                var attendance = await _attendanceReportRepository.GetSingleAsync(x => x.Id == request.ReportId);
                if (attendance == null) throw new NotFoundException(nameof(AttendanceReport), Constants.ErrorCode_ReportNotFound + $"Report with Id {request.ReportId} not found.");

                var updateObj = _mapper.Map<AttendanceReport>(request);
                await _attendanceReportRepository.UpdateAsync(updateObj);

                response.Success = true;
                response.Message = Constants.SuccessResponse;
            }
            catch (CustomException ex)
            {
                response.Message = ex.Message;
            }
            catch (NotFoundException e)
            {
                response.Message = e.Message;
                response.Success = false;
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