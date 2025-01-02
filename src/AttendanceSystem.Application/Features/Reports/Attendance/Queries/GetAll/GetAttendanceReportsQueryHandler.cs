using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Helpers;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Reports.Attendance.Queries.GetAll
{
    public class GetAttendanceReportsQueryHandler : IRequestHandler<GetAttendanceReportsQuery, GetAttendanceReportsQueryResponse>
    {
        private readonly ILogger<GetAttendanceReportsQueryHandler> _logger;
        private readonly IAsyncRepository<AttendanceReport> _attendanceReportRepository;
        private readonly IMapper _mapper;
        public GetAttendanceReportsQueryHandler(ILogger<GetAttendanceReportsQueryHandler> logger, IAsyncRepository<AttendanceReport> attendanceReportRepository, IMapper mapper)
        {
            _logger = logger;
            _attendanceReportRepository = attendanceReportRepository;
            _mapper = mapper;
        }

        public async Task<GetAttendanceReportsQueryResponse> Handle(GetAttendanceReportsQuery request, CancellationToken cancellationToken)
        {
            var response = new GetAttendanceReportsQueryResponse();
            try
            {
                var filter = PredicateBuilder.True<AttendanceReport>();

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

                var pagedResult = await _attendanceReportRepository.GetPagedFilteredAsync(filter, request.Page, request.PageSize, request.SortColumn,
                    request.SortOrder, false, x => x.Member, x => x.Activity);

                var result = _mapper.Map<PagedResult<AttendanceReportListResultVM>>(pagedResult);

                response.Result = result;
                response.Success = true;
                response.Message = Constants.SuccessResponse;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex.ToString() + "{Microservice}", Constants.Microservice);
                response.Message = $"Error completing attendance report querying request.";
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