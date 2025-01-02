using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Features.Reports.Outreach.Commands.Create;
using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Reports.Outreach.Commands.Create
{
    public class CreateOutreachReportCommandHandler : IRequestHandler<CreateOutreachReportCommand, BaseResponse>
    {
        private readonly IAsyncRepository<OutreachReport> _outreachReportRepository;
        private readonly ILogger<CreateOutreachReportCommandHandler> _logger;
        private readonly IAsyncRepository<Member> _memberRepository;
        private readonly IAsyncRepository<Activity> _activityRepository;
        private readonly IMapper _mapper;
        public CreateOutreachReportCommandHandler(IAsyncRepository<OutreachReport> outreachReportRepository, ILogger<CreateOutreachReportCommandHandler> logger,
            IAsyncRepository<Member> memberRepository, IAsyncRepository<Activity> activityRepository, IMapper mapper)
        {
            _outreachReportRepository = outreachReportRepository;
            _logger = logger;
            _memberRepository = memberRepository;
            _activityRepository = activityRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse> Handle(CreateOutreachReportCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();
            try
            {
                var validator = new CreateOutreachReportCommandValidator(_memberRepository, _activityRepository);
                var validationResult = await validator.ValidateAsync(request);
                if (validationResult.Errors.Count > 0)
                    throw new ValidationException(validationResult);

                var report = _mapper.Map<OutreachReport>(request);

                await _outreachReportRepository.AddAsync(report);

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