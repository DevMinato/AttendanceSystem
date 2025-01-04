using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace AttendanceSystem.Application.Features.Reports.Outreach.Queries.GetSingle
{
    public class GetOutreachReportQueryHandler : IRequestHandler<GetOutreachReportQuery, GetOutreachReportQueryResponse>
    {
        private readonly ILogger<GetOutreachReportQueryHandler> _logger;
        private readonly IAsyncRepository<OutreachReport> _outreachReportRepository;
        private readonly IAsyncRepository<OutreachDetail> _outreachDetailRepository;
        private readonly IMapper _mapper;
        public GetOutreachReportQueryHandler(ILogger<GetOutreachReportQueryHandler> logger, IAsyncRepository<OutreachReport> outreachReportRepository, IMapper mapper,
            IAsyncRepository<OutreachDetail> outreachDetailRepository)
        {
            _logger = logger;
            _outreachReportRepository = outreachReportRepository;
            _mapper = mapper;
            _outreachDetailRepository = outreachDetailRepository;
        }

        public async Task<GetOutreachReportQueryResponse> Handle(GetOutreachReportQuery request, CancellationToken cancellationToken)
        {
            var response = new GetOutreachReportQueryResponse();
            try
            {
                var includeExpressions = new Expression<Func<OutreachReport, object>>[]
                {
                    report => report.OutreachDetails,
                    report => report.Member,
                    report => report.Activity,
                };

                var report = await _outreachReportRepository.GetSingleAsync(x => x.Id == request.ReportId, false, includeExpressions);
                if (report == null)
                {
                    throw new NotFoundException(nameof(FollowUpReport), Constants.ErrorCode_ReportNotFound + $" Report with request id {request.ReportId} not found");
                }

                response.Result = _mapper.Map<OutreachReportDetailResultVM>(report);
                response.Success = true;
                response.Message = Constants.SuccessResponse;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex.ToString() + "{Microservice}", Constants.Microservice);
                response.Message = $"Error completing outreach report querying request.";
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                response.Success = false;
                response.Message = ex.Message;
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