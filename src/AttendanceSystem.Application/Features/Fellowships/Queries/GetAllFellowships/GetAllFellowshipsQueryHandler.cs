using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Helpers;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Fellowships.Queries.GetAllFellowships
{
    public class GetAllFellowshipsQueryHandler : IRequestHandler<GetAllFellowshipsQuery, GetAllFellowshipsQueryResponse>
    {
        private readonly ILogger<GetAllFellowshipsQueryHandler> _logger;
        private readonly IAsyncRepository<Fellowship> _fellowshipRepository;
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Pastor> _pastorRepository;
        public GetAllFellowshipsQueryHandler(ILogger<GetAllFellowshipsQueryHandler> logger, IAsyncRepository<Fellowship> fellowshipRepository, IMapper mapper, IAsyncRepository<Pastor> pastorRepository)
        {
            _logger = logger;
            _fellowshipRepository = fellowshipRepository;
            _mapper = mapper;
            _pastorRepository = pastorRepository;
        }

        public async Task<GetAllFellowshipsQueryResponse> Handle(GetAllFellowshipsQuery request, CancellationToken cancellationToken)
        {
            var response = new GetAllFellowshipsQueryResponse();
            try
            {
                var filter = PredicateBuilder.True<Fellowship>();

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
                    filter = filter.And(c => c.Name.ToLower().Contains(request.Search.ToLower()));
                }

                var pagedResult = await _fellowshipRepository.GetPagedFilteredAsync(filter, request.Page, request.PageSize, request.SortColumn,
                    request.SortOrder, false);

                var result = _mapper.Map<PagedResult<FellowshipsListResultVM>>(pagedResult);

                var pastorIds = pagedResult.Items.Select(x => x.PastorId).ToList();

                var pastors = await _pastorRepository.GetFilteredAsync(x => pastorIds.Contains(x.Id));

                if (result.Items.Count > 0)
                {
                    foreach (var item in result.Items)
                    {
                        var pastor = pastors?.FirstOrDefault(x => x.Id == item.PastorId);
                        if(pastor != null) item.PastorFullName = $"{pastor.FirstName} {pastor.LastName}";

                        if(item.ParentId != null)
                        {
                            var fellowship = await _fellowshipRepository.GetSingleAsync(x => x.Id == item.ParentId);
                            if (fellowship != null) item.ParentFellowship = $"{fellowship.Name}";
                        }
                    }
                }

                response.Result = result;
                response.Success = true;
                response.Message = Constants.SuccessResponse;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex.ToString() + "{Microservice}", Constants.Microservice);
                response.Message = $"Error completing fellowship querying request.";
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