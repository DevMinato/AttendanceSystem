using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Responses;
using AttendanceSystem.Application.Utilities;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Reports.Followup.Commands.Create
{
    public class CreateFollowupReportCommandHandler : IRequestHandler<CreateFollowupReportCommand, BaseResponse>
    {
        private readonly IAsyncRepository<FollowUpReport> _followupReportRepository;
        private readonly IAsyncRepository<FollowUpDetail> _followupDetailRepository;
        private readonly ILogger<CreateFollowupReportCommandHandler> _logger;
        private readonly IAsyncRepository<Member> _memberRepository;
        private readonly IAsyncRepository<Activity> _activityRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public CreateFollowupReportCommandHandler(IAsyncRepository<FollowUpReport> followupReportRepository, ILogger<CreateFollowupReportCommandHandler> logger,
            IAsyncRepository<Member> memberRepository, IAsyncRepository<Activity> activityRepository, IMapper mapper, IAsyncRepository<FollowUpDetail> followupDetailRepository, IUserService userService)
        {
            _followupReportRepository = followupReportRepository;
            _logger = logger;
            _memberRepository = memberRepository;
            _activityRepository = activityRepository;
            _mapper = mapper;
            _followupDetailRepository = followupDetailRepository;
            _userService = userService;
        }

        public async Task<BaseResponse> Handle(CreateFollowupReportCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();
            try
            {
                var validator = new CreateFollowupReportCommandValidator(_memberRepository, _activityRepository);
                var validationResult = await validator.ValidateAsync(request);
                if (validationResult.Errors.Count > 0)
                    throw new ValidationException(validationResult);

                //var report = _mapper.Map<FollowUpReport>(request);
                if (request.FollowUpDetails.Count > 0)
                {
                    var followupObj = new FollowUpReport
                    {
                        MemberId = _userService.UserDetails().UserId,
                        ActivityId = request.FollowUpDetails.First().ActivityId.Value,
                        TotalFollowUps = request.FollowUpDetails.Count
                    };
                    await _followupReportRepository.AddAsync(followupObj);

                    List<FollowUpDetail> followUpDetails = new List<FollowUpDetail>();

                    foreach (var detail in request.FollowUpDetails)
                    {
                        var detailObj = new FollowUpDetail
                        {
                            FullName = detail.FullName,
                            MemberId = detail.MemberId.Value,
                            FollowUpType = detail.FollowUpType,
                            FollowUpReportId = followupObj.Id,
                            Notes = detail.Notes,
                            Date = detail.Date,
                        };

                        followUpDetails.Add(detailObj);
                    }

                    await _followupDetailRepository.AddRangeAsync(followUpDetails);
                }


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