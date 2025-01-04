using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Helpers;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Pastors.Queries.GetAllPastors
{
    public class GetAllPastorsQueryHandler : IRequestHandler<GetAllPastorsQuery, GetAllPastorsQueryResponse>
    {
        private readonly ILogger<GetAllPastorsQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Pastor> _pastorRepository;
        public GetAllPastorsQueryHandler(ILogger<GetAllPastorsQueryHandler> logger, IMapper mapper, IAsyncRepository<Pastor> pastorRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _pastorRepository = pastorRepository;
        }

        public async Task<GetAllPastorsQueryResponse> Handle(GetAllPastorsQuery request, CancellationToken cancellationToken)
        {
            var response = new GetAllPastorsQueryResponse();
            try
            {
                var filter = PredicateBuilder.True<Pastor>();

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
                    filter = filter.And(c => c.FirstName.ToLower().Contains(request.Search.ToLower()) || c.LastName.ToLower().Contains(request.Search.ToLower()));
                }

                var pagedResult = await _pastorRepository.GetPagedFilteredAsync(filter, request.Page, request.PageSize, request.SortColumn,
                    request.SortOrder, false, x => x.Fellowship);

                var result = _mapper.Map<PagedResult<PastorsListResultVM>>(pagedResult);

                response.Result = result;
                response.Success = true;
                response.Message = Constants.SuccessResponse;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex.ToString() + "{Microservice}", Constants.Microservice);
                response.Message = $"Error completing pastor querying request.";
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