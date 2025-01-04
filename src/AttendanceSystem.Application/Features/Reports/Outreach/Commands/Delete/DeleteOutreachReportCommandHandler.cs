using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Reports.Outreach.Commands.Delete
{
    public class DeleteOutreachReportCommandHandler : IRequestHandler<DeleteOutreachReportCommand, BaseResponse>
    {
        private readonly IAsyncRepository<OutreachReport> _outreachReportRepository;
        private readonly ILogger<DeleteOutreachReportCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteOutreachReportCommandHandler(IAsyncRepository<OutreachReport> outreachReportRepository, ILogger<DeleteOutreachReportCommandHandler> logger,
             IAsyncRepository<Activity> activityRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _outreachReportRepository = outreachReportRepository;
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Handle(DeleteOutreachReportCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();
            try
            {
                var validator = new DeleteOutreachReportCommandValidator(_outreachReportRepository);
                var validationResult = await validator.ValidateAsync(request);
                if (validationResult.Errors.Count > 0)
                    throw new ValidationException(validationResult);

                var report = await _outreachReportRepository.GetSingleAsync(x => x.Id == request.ReportId);
                if (report == null) throw new NotFoundException(nameof(FollowUpReport), $" Report with Id {request.ReportId} not found, ({Constants.ErrorCode_ReportNotFound})");

                _unitOfWork.BeginTransaction();

                _unitOfWork.OutreachReportRepository.Update(report);

                foreach (var detail in report.OutreachDetails)
                {
                    detail.IsDeleted = true;
                    _unitOfWork.OutreachDetailRepository.Update(detail);
                }

                _unitOfWork.Commit();
                _unitOfWork.Dispose();

                report.IsDeleted = true;
                await _outreachReportRepository.UpdateAsync(report);

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