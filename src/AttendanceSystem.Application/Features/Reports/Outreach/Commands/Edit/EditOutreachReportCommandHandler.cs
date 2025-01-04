using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace AttendanceSystem.Application.Features.Reports.Outreach.Commands.Edit
{
    public class EditOutreachReportCommandHandler : IRequestHandler<EditOutreachReportCommand, BaseResponse>
    {
        private readonly ILogger<EditOutreachReportCommandHandler> _logger;
        private readonly IAsyncRepository<OutreachReport> _outreachReportRepository;
        private readonly IAsyncRepository<Member> _memberRepository;
        private readonly IAsyncRepository<Activity> _activityRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public EditOutreachReportCommandHandler(ILogger<EditOutreachReportCommandHandler> logger, IAsyncRepository<OutreachReport> outreachReportRepository,
            IMapper mapper, IAsyncRepository<Member> memberRepository, IAsyncRepository<Activity> activityRepository, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _outreachReportRepository = outreachReportRepository;
            _mapper = mapper;
            _memberRepository = memberRepository;
            _activityRepository = activityRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Handle(EditOutreachReportCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();
            try
            {
                var validator = new EditOutreachReportCommandValidator(_outreachReportRepository, _memberRepository, _activityRepository);
                var validationResult = await validator.ValidateAsync(request);
                if (validationResult.Errors.Count > 0)
                    throw new ValidationException(validationResult);

                var includeExpressions = new Expression<Func<OutreachReport, object>>[]
                {
                    report => report.OutreachDetails
                };
                var report = await _outreachReportRepository.GetSingleAsync(x => x.Id == request.ReportId, false, includeExpressions);
                if (report == null) throw new NotFoundException(nameof(OutreachReport), $" Report with Id {request.ReportId} not found, ({Constants.ErrorCode_ReportNotFound})");

                _unitOfWork.BeginTransaction();

                _unitOfWork.OutreachDetailRepository.DeleteRange(report.OutreachDetails);

                _mapper.Map(request, report, typeof(EditOutreachReportCommand), typeof(OutreachReport));

                _unitOfWork.OutreachReportRepository.Update(report);

                foreach (var details in report.OutreachDetails)
                {
                    _unitOfWork.OutreachDetailRepository.Update(details);
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