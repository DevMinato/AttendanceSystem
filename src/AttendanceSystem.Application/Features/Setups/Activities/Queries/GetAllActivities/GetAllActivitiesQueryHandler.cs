using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Helpers;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Setups.Activities.Queries.GetAllActivities
{
    public class GetAllActivitiesQueryHandler : IRequestHandler<GetAllActivitiesQuery, GetAllActivitiesQueryResponse>
    {
        private readonly ILogger<GetAllActivitiesQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Activity> _activityRepository;
        public GetAllActivitiesQueryHandler(ILogger<GetAllActivitiesQueryHandler> logger, IMapper mapper, IAsyncRepository<Activity> activityRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _activityRepository = activityRepository;
        }

        public async Task<GetAllActivitiesQueryResponse> Handle(GetAllActivitiesQuery request, CancellationToken cancellationToken)
        {
            var response = new GetAllActivitiesQueryResponse();
            try
            {
                var filter = PredicateBuilder.True<Activity>();

                if (request.StartDate.HasValue)
                {
                    filter = filter.And(c => c.CreatedAt >= request.StartDate.Value);
                }
                if (request.EndDate.HasValue)
                {
                    var eDate = request.EndDate.Value.AddDays(1).AddSeconds(-1);
                    filter = filter.And(c => c.CreatedAt <= eDate);
                }
                if (!string.IsNullOrWhiteSpace(request.Search))
                {
                    filter = filter.And(c => c.Name.ToLower().Contains(request.Search.ToLower()) || c.Description.ToLower().Contains(request.Search.ToLower()));
                }

                var pagedResult = await _activityRepository.GetPagedFilteredAsync(filter, request.Page, request.PageSize, request.SortColumn,
                    request.SortOrder, false, x => x.ActivityReports);

                var result = _mapper.Map<PagedResult<ActivitiesListResultVM>>(pagedResult);

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