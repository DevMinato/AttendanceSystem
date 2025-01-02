using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Features.Reports.Attendance.Commands.Create;
using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Reports.Attendance.Commands.Delete
{
    public class DeleteAttendanceCommandHandler : IRequestHandler<DeleteAttendanceCommand, BaseResponse>
    {
        private readonly IAsyncRepository<AttendanceReport> _attendanceReportRepository;
        private readonly ILogger<DeleteAttendanceCommandHandler> _logger;
        private readonly IMapper _mapper;
        public DeleteAttendanceCommandHandler(IAsyncRepository<AttendanceReport> attendanceReportRepository, ILogger<DeleteAttendanceCommandHandler> logger,
            IAsyncRepository<Activity> activityRepository, IMapper mapper)
        {
            _attendanceReportRepository = attendanceReportRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<BaseResponse> Handle(DeleteAttendanceCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();
            try
            {
                var validator = new DeleteAttendanceCommandValidator(_attendanceReportRepository);
                var validationResult = await validator.ValidateAsync(request);
                if (validationResult.Errors.Count > 0)
                    throw new ValidationException(validationResult);

                var report = await _attendanceReportRepository.GetSingleAsync(x => x.Id == request.ReportId);
                if (report == null) throw new NotFoundException(nameof(AttendanceReport), Constants.ErrorCode_ReportNotFound + $"Report with Id {request.ReportId} not found.");

                report.IsDeleted = true;
                await _attendanceReportRepository.UpdateAsync(report);

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
            catch (NotFoundException e)
            {
                response.Message = e.Message;
                response.Success = false;
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