using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Helpers;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace AttendanceSystem.Application.Features.Reports.Followup.Queries.GetAll
{
    public class GetFollowupReportsQueryHandler : IRequestHandler<GetFollowupReportsQuery, GetFollowupReportsQueryResponse>
    {
        private readonly ILogger<GetFollowupReportsQueryHandler> _logger;
        private readonly IAsyncRepository<FollowUpReport> _followupReportRepository;
        private readonly IAsyncRepository<FollowUpDetail> _followupDetailRepository;
        private readonly IMapper _mapper;
        public GetFollowupReportsQueryHandler(ILogger<GetFollowupReportsQueryHandler> logger, IAsyncRepository<FollowUpReport> followupReportRepository, IMapper mapper,
            IAsyncRepository<FollowUpDetail> followupDetailRepository)
        {
            _logger = logger;
            _followupReportRepository = followupReportRepository;
            _mapper = mapper;
            _followupDetailRepository = followupDetailRepository;
        }

        public async Task<GetFollowupReportsQueryResponse> Handle(GetFollowupReportsQuery request, CancellationToken cancellationToken)
        {
            var response = new GetFollowupReportsQueryResponse();
            try
            {
                var filter = PredicateBuilder.True<FollowUpReport>();

                if (request.MemberId.HasValue)
                {
                    filter = filter.And(c => c.MemberId == request.MemberId.Value);
                }

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
                    filter = filter.And(c => c.Member.FirstName.ToLower().Contains(request.Search.ToLower())
                    || c.Member.LastName.ToLower().Contains(request.Search.ToLower())
                    || c.Activity.Name.ToLower().Contains(request.Search.ToLower()));
                }

                var includeExpressions = new Expression<Func<FollowUpReport, object>>[]
                {
                    report => report.FollowUpDetails,
                    report => report.Member,
                    report => report.Activity,
                };

                var pagedResult = await _followupReportRepository.GetPagedFilteredAsync(filter, request.Page, request.PageSize, request.SortColumn,
                    request.SortOrder, false, includeExpressions);

                var result = _mapper.Map<PagedResult<FollowupReportListResultVM>>(pagedResult);

                response.Result = result;
                response.Success = true;
                response.Message = Constants.SuccessResponse;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex.ToString() + "{Microservice}", Constants.Microservice);
                response.Message = $"Error completing follow-up report querying request.";
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