using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Helpers;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace AttendanceSystem.Application.Features.StudyGroup.Queries.GetAll
{
    public class GetAllStudyGroupSubmissionsQueryHandler : IRequestHandler<GetAllStudyGroupSubmissionsQuery, GetAllStudyGroupSubmissionsQueryResponse>
    {
        private readonly ILogger<GetAllStudyGroupSubmissionsQueryHandler> _logger;
        private readonly IAsyncRepository<StudyGroupSubmission> _studyGroupSubmissionRepository;
        private readonly IAsyncRepository<Member> _memberRepository;
        private readonly IMapper _mapper;
        public GetAllStudyGroupSubmissionsQueryHandler(ILogger<GetAllStudyGroupSubmissionsQueryHandler> logger,
            IAsyncRepository<StudyGroupSubmission> studyGroupSubmissionRepository, IMapper mapper, IAsyncRepository<Member> memberRepository)
        {
            _logger = logger;
            _studyGroupSubmissionRepository = studyGroupSubmissionRepository;
            _mapper = mapper;
            _memberRepository = memberRepository;
        }

        public async Task<GetAllStudyGroupSubmissionsQueryResponse> Handle(GetAllStudyGroupSubmissionsQuery request, CancellationToken cancellationToken)
        {
            var response = new GetAllStudyGroupSubmissionsQueryResponse();
            try
            {
                var filter = PredicateBuilder.True<StudyGroupSubmission>();

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
                    filter = filter.And(c => c.StudyGroup.StudyGroupMaterial.ToLower().Contains(request.Search.ToLower()) || c.StudyGroup.StudyGroupQuestion.Contains(request.Search.ToLower()));
                }

                var includeExpressions = new Expression<Func<StudyGroupSubmission, object>>[]
                {
                    report => report.StudyGroup
                };

                var pagedResult = await _studyGroupSubmissionRepository.GetPagedFilteredAsync(filter, request.Page, request.PageSize, request.SortColumn,
                    request.SortOrder, false, includeExpressions);

                var result = _mapper.Map<PagedResult<StudyGroupSubmissionsListResultVM>>(pagedResult);

                var memberIds = pagedResult.Items.Select(x => x.MemberId).ToList();

                var members = await _memberRepository.GetFilteredAsync(x => memberIds.Contains(x.Id));

                if (result.Items.Count > 0)
                {
                    foreach (var item in result.Items)
                    {
                        var member = members?.FirstOrDefault(x => x.Id == item.MemberId);
                        if (member != null) item.MemberFullName = $"{member.FirstName} {member.LastName}";
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