using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Helpers;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Members.Queries.GetAllMembers
{
    public class GetAllMembersQueryHandler : IRequestHandler<GetAllMembersQuery, GetAllMembersQueryResponse>
    {
        private readonly ILogger<GetAllMembersQueryHandler> _logger;
        private readonly IAsyncRepository<Member> _memberRepository;
        private readonly IMapper _mapper;
        public GetAllMembersQueryHandler(ILogger<GetAllMembersQueryHandler> logger, IAsyncRepository<Member> memberRepository, IMapper mapper)
        {
            _logger = logger;
            _memberRepository = memberRepository;
            _mapper = mapper;
        }

        public async Task<GetAllMembersQueryResponse> Handle(GetAllMembersQuery request, CancellationToken cancellationToken)
        {
            var response = new GetAllMembersQueryResponse();
            try
            {
                var filter = PredicateBuilder.True<Member>();

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
                if (request.Gender.HasValue)
                {
                    filter = filter.And(c => c.Gender.Equals(request.Gender));
                }
                if (request.FellowshipId.HasValue)
                {
                    filter = filter.And(c => c.FellowshipId == request.FellowshipId);
                }
                var pagedResult = await _memberRepository.GetPagedFilteredAsync(filter, request.Page, request.PageSize, request.SortColumn,
                    request.SortOrder, false, x => x.Fellowship);

                var result = _mapper.Map<PagedResult<MembersListResultVM>>(pagedResult);

                var disciplerIds = pagedResult.Items.Select(x => x.DisciplerId).ToList();

                var members = await _memberRepository.GetFilteredAsync(x => disciplerIds.Contains(x.Id));

                if (result.Items.Count > 0)
                {
                    foreach (var item in result.Items)
                    {
                        var member = members?.FirstOrDefault(members => members.Id == item.DisciplerId);
                        if(member != null) item.DisciplerFullName = $"{member.FirstName} {member.LastName}";
                    }
                }

                response.Result = result;
                response.Success = true;
                response.Message = Constants.SuccessResponse;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex.ToString() + "{Microservice}", Constants.Microservice);
                response.Message = $"Error completing member querying request.";
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