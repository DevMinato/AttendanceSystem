using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace AttendanceSystem.Application.Features.Reports.Followup.Commands.Edit
{
    public class EditFollowupReportCommandHandler : IRequestHandler<EditFollowupReportCommand, BaseResponse>
    {
        private readonly ILogger<EditFollowupReportCommandHandler> _logger;
        private readonly IAsyncRepository<FollowUpReport> _followupReportRepository;
        private readonly IAsyncRepository<Member> _memberRepository;
        private readonly IAsyncRepository<Activity> _activityRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public EditFollowupReportCommandHandler(ILogger<EditFollowupReportCommandHandler> logger, IAsyncRepository<FollowUpReport> followupReportRepository,
            IMapper mapper, IAsyncRepository<Member> memberRepository, IAsyncRepository<Activity> activityRepository, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _followupReportRepository = followupReportRepository;
            _mapper = mapper;
            _memberRepository = memberRepository;
            _activityRepository = activityRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Handle(EditFollowupReportCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();
            try
            {
                var validator = new EditFollowupReportCommandValidator(_followupReportRepository, _memberRepository, _activityRepository);
                var validationResult = await validator.ValidateAsync(request);
                if (validationResult.Errors.Count > 0)
                    throw new ValidationException(validationResult);

                var includeExpressions = new Expression<Func<FollowUpReport, object>>[]
                {
                    report => report.FollowUpDetails
                };
                var report = await _followupReportRepository.GetSingleAsync(x => x.Id == request.ReportId, false, includeExpressions);
                if (report == null) throw new NotFoundException(nameof(FollowUpReport), $"Report with Id {request.ReportId} not found, ({Constants.ErrorCode_ReportNotFound})");

                _unitOfWork.BeginTransaction();

                _unitOfWork.FollowupDetailRepository.DeleteRange(report.FollowUpDetails);

                _mapper.Map(request, report, typeof(EditFollowupReportCommand), typeof(FollowUpReport));

                _unitOfWork.FollowupReportRepository.Update(report);

                foreach (var details in report.FollowUpDetails)
                {
                    _unitOfWork.FollowupDetailRepository.Update(details);
                }

                _unitOfWork.Commit();
                _unitOfWork.Dispose();

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