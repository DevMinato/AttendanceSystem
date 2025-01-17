using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Reports.Followup.Commands.Delete
{
    public class DeleteFollowupReportCommandHandler : IRequestHandler<DeleteFollowupReportCommand, BaseResponse>
    {
        private readonly IAsyncRepository<FollowUpReport> _followupReportRepository;
        private readonly ILogger<DeleteFollowupReportCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteFollowupReportCommandHandler(IAsyncRepository<FollowUpReport> followupReportRepository, ILogger<DeleteFollowupReportCommandHandler> logger,
             IAsyncRepository<Activity> activityRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _followupReportRepository = followupReportRepository;
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Handle(DeleteFollowupReportCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();
            try
            {
                var validator = new DeleteFollowupReportCommandValidator(_followupReportRepository);
                var validationResult = await validator.ValidateAsync(request);
                if (validationResult.Errors.Count > 0)
                    throw new ValidationException(validationResult);

                var report = await _followupReportRepository.GetSingleAsync(x => x.Id == request.ReportId);
                if (report == null) throw new NotFoundException(nameof(FollowUpReport), $" Report with Id {request.ReportId} not found, ({Constants.ErrorCode_ReportNotFound})");

                _unitOfWork.BeginTransaction();

                _unitOfWork.FollowupReportRepository.Update(report);

                foreach (var detail in report.FollowUpDetails)
                {
                    detail.IsDeleted = true;
                    _unitOfWork.FollowupDetailRepository.Update(detail);
                }

                report.IsDeleted = true;
                await _followupReportRepository.UpdateAsync(report);

                _unitOfWork.Commit();
                _unitOfWork.Dispose();

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