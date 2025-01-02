using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Reports.Attendance.Queries.GetSingle
{
    public class GetAttendanceReportQueryHandler : IRequestHandler<GetAttendanceReportQuery, GetAttendanceReportQueryResponse>
    {
        private readonly ILogger<GetAttendanceReportQueryHandler> _logger;
        private readonly IAsyncRepository<AttendanceReport> _attendanceReportRepository;
        private readonly IMapper _mapper;
        public GetAttendanceReportQueryHandler(ILogger<GetAttendanceReportQueryHandler> logger, IAsyncRepository<AttendanceReport> attendanceReportRepository, IMapper mapper)
        {
            _logger = logger;
            _attendanceReportRepository = attendanceReportRepository;
            _mapper = mapper;
        }

        public async Task<GetAttendanceReportQueryResponse> Handle(GetAttendanceReportQuery request, CancellationToken cancellationToken)
        {
            var response = new GetAttendanceReportQueryResponse();
            try
            {
                var report = await _attendanceReportRepository.GetSingleAsync(x => x.Id == request.ReportId, false, x => x.Activity, x => x.Member);
                if (report == null)
                {
                    throw new NotFoundException(nameof(AttendanceReport), Constants.ErrorCode_ReportNotFound + $"Report with request id {request.ReportId} not found");
                }

                var result = _mapper.Map<AttendanceReportDetailResultVM>(report);

                response.Result = result;
                response.Success = true;
                response.Message = Constants.SuccessResponse;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex.ToString() + "{Microservice}", Constants.Microservice);
                response.Message = $"Error completing attendance report querying request.";
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