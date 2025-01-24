using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Helpers;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Setups.StudyGroup.Queries.GetAllStudyGroups
{
    public class GetAllStudyGroupsQueryHandler : IRequestHandler<GetAllStudyGroupsQuery, GetAllStudyGroupsQueryResponse>
    {
        private readonly ILogger<GetAllStudyGroupsQueryHandler> _logger;
        private readonly IAsyncRepository<Domain.Entities.StudyGroup> _studyGroupRepository;
        private readonly IMapper _mapper;
        public GetAllStudyGroupsQueryHandler(ILogger<GetAllStudyGroupsQueryHandler> logger, IAsyncRepository<Domain.Entities.StudyGroup> studyGroupRepository, IMapper mapper)
        {
            _logger = logger;
            _studyGroupRepository = studyGroupRepository;
            _mapper = mapper;
        }

        public async Task<GetAllStudyGroupsQueryResponse> Handle(GetAllStudyGroupsQuery request, CancellationToken cancellationToken)
        {
            var response = new GetAllStudyGroupsQueryResponse();
            try
            {
                var filter = PredicateBuilder.True<Domain.Entities.StudyGroup>();

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
                    filter = filter.And(c => c.StudyGroupMaterial.ToLower().Contains(request.Search.ToLower()) || c.StudyGroupQuestion.Contains(request.Search.ToLower()));
                }

                var pagedResult = await _studyGroupRepository.GetPagedFilteredAsync(filter, request.Page, request.PageSize, request.SortColumn,
                    request.SortOrder, false);

                var result = _mapper.Map<PagedResult<StudyGroupsListResultVM>>(pagedResult);

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