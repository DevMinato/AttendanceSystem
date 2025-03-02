using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using AutoMapper.Execution;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace AttendanceSystem.Application.Features.Reports.Followup.Queries.GetSingle
{
    public class GetFollowupReportQueryHandler : IRequestHandler<GetFollowupReportQuery, GetFollowupReportQueryResponse>
    {
        private readonly ILogger<GetFollowupReportQueryHandler> _logger;
        private readonly IAsyncRepository<FollowUpReport> _followupReportRepository;
        private readonly IAsyncRepository<FollowUpDetail> _followupDetailRepository;
        private readonly IAsyncRepository<Domain.Entities.Member> _memberRepository;
        private readonly IMapper _mapper;
        public GetFollowupReportQueryHandler(ILogger<GetFollowupReportQueryHandler> logger, IAsyncRepository<FollowUpReport> followupReportRepository, IMapper mapper,
            IAsyncRepository<FollowUpDetail> followupDetailRepository, IAsyncRepository<Domain.Entities.Member> memberRepository)
        {
            _logger = logger;
            _followupReportRepository = followupReportRepository;
            _mapper = mapper;
            _followupDetailRepository = followupDetailRepository;
            _memberRepository = memberRepository;
        }

        public async Task<GetFollowupReportQueryResponse> Handle(GetFollowupReportQuery request, CancellationToken cancellationToken)
        {
            var response = new GetFollowupReportQueryResponse();
            try
            {
                var report = await _followupDetailRepository.GetFilteredAsync(x => x.FollowUpReportId == request.ReportId, false);
                if (report == null)
                {
                    throw new NotFoundException(nameof(FollowUpReport), Constants.ErrorCode_ReportNotFound + $" Report with request id {request.ReportId} not found");
                }

                var result = _mapper.Map<List<FollowupReportDetailResultVM>>(report);

                if (result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        var disciple = await _memberRepository.GetSingleAsync(members => members.Id == item.DiscipleId);
                        if (disciple != null) item.DiscipleFullName = $"{disciple.FirstName} {disciple.LastName}";
                    }
                }

                response.Success = true;
                response.Message = Constants.SuccessResponse;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex.ToString() + "{Microservice}", Constants.Microservice);
                response.Message = $"Error completing follow-up report querying request.";
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