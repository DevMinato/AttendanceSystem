using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Helpers;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace AttendanceSystem.Application.Features.Reports.Outreach.Queries.GetAll
{
    public class GetOutreachReportsQueryHandler : IRequestHandler<GetOutreachReportsQuery, GetOutreachReportsQueryResponse>
    {
        private readonly ILogger<GetOutreachReportsQueryHandler> _logger;
        private readonly IAsyncRepository<OutreachReport> _outreachReportRepository;
        private readonly IAsyncRepository<OutreachDetail> _outreachDetailRepository;
        private readonly IMapper _mapper;
        public GetOutreachReportsQueryHandler(ILogger<GetOutreachReportsQueryHandler> logger, IAsyncRepository<OutreachReport> outreachReportRepository, IMapper mapper,
            IAsyncRepository<OutreachDetail> outreachDetailRepository)
        {
            _logger = logger;
            _outreachReportRepository = outreachReportRepository;
            _mapper = mapper;
            _outreachDetailRepository = outreachDetailRepository;
        }

        public async Task<GetOutreachReportsQueryResponse> Handle(GetOutreachReportsQuery request, CancellationToken cancellationToken)
        {
            var response = new GetOutreachReportsQueryResponse();
            try
            {
                var filter = PredicateBuilder.True<OutreachReport>();

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

                var includeExpressions = new Expression<Func<OutreachReport, object>>[]
                {
                    report => report.OutreachDetails,
                    report => report.Member,
                    report => report.Activity,
                };

                var pagedResult = await _outreachReportRepository.GetPagedFilteredAsync(filter, request.Page, request.PageSize, request.SortColumn,
                    request.SortOrder, false, includeExpressions);

                var result = _mapper.Map<PagedResult<OutreachReportListResultVM>>(pagedResult);

                response.Result = result;
                response.Success = true;
                response.Message = Constants.SuccessResponse;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex.ToString() + "{Microservice}", Constants.Microservice);
                response.Message = $"Error completing outreach report querying request.";
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