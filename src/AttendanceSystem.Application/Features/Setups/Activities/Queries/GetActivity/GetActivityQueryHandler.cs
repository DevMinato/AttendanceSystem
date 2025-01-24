using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Setups.Activities.Queries.GetActivity
{
    public class GetActivityQueryHandler : IRequestHandler<GetActivityQuery, GetActivityQueryResponse>
    {
        private readonly ILogger<GetActivityQueryHandler> _logger;
        private readonly IAsyncRepository<Activity> _activityRepository;
        private readonly IMapper _mapper;
        public GetActivityQueryHandler(ILogger<GetActivityQueryHandler> logger, IMapper mapper, IAsyncRepository<Activity> activityRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _activityRepository = activityRepository;
        }

        public async Task<GetActivityQueryResponse> Handle(GetActivityQuery request, CancellationToken cancellationToken)
        {
            var response = new GetActivityQueryResponse();
            try
            {
                var pastor = await _activityRepository.GetSingleAsync(x => x.Id == request.Id, false);
                if (pastor == null)
                {
                    throw new CustomException(Constants.ErrorCode_RecordNotFound + $" Activity with request id {request.Id} not found.");
                }

                var result = _mapper.Map<ActivityDetailResultVM>(pastor);

                response.Result = result;
                response.Success = true;
                response.Message = Constants.SuccessResponse;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex.ToString() + "{Microservice}", Constants.Microservice);
                response.Message = $"Error completing activity querying request.";
            }
            catch (CustomException ex)
            {
                _logger.LogError(ex.ToString(), ex.Message);
                response.Message = ex.Message;
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