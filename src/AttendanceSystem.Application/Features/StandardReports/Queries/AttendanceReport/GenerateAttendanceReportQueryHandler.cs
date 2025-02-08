using AttendanceSystem.Application.Contracts.Infrastructure;
using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Utilities;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Enums;
using AttendanceSystem.Domain.Extensions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.StandardReports.Queries.AttendanceReport
{
    public class GenerateAttendanceReportQueryHandler : IRequestHandler<GenerateAttendanceReportQuery, GenerateAttendanceReportQueryResponse>
    {
        private readonly ILogger<GenerateAttendanceReportQueryHandler> _logger;
        private readonly IUserService _userService;
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Fellowship> _fellowshipRepository;

        public GenerateAttendanceReportQueryHandler(ILogger<GenerateAttendanceReportQueryHandler> logger, IUserService userService,
            IReportRepository reportRepository, IAsyncRepository<Fellowship> fellowshipRepository, IMapper mapper)
        {
            _logger = logger;
            _userService = userService;
            _reportRepository = reportRepository;
            _mapper = mapper;
            _fellowshipRepository = fellowshipRepository;
        }

        public async Task<GenerateAttendanceReportQueryResponse> Handle(GenerateAttendanceReportQuery request, CancellationToken cancellationToken)
        {
            var response = new GenerateAttendanceReportQueryResponse();
            List<Guid> activityIds = new List<Guid>();

            try
            {
                if (_userService?.UserDetails()?.UserType == MemberType.Pastor.DisplayName())
                {
                    if (!request.StartDate.HasValue)
                        request.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    if (!request.EndDate.HasValue)
                        request.EndDate = DateTime.Now;

                    if (!string.IsNullOrEmpty(request.ActivityIds))
                    {
                        activityIds = request.ActivityIds
                            .Split(',')
                            .Select(Guid.Parse)
                            .ToList();
                    }

                    var reports = await _reportRepository.FetchAttendanceData(request.StartDate.Value, request.EndDate.Value, activityIds);

                    var groupedReports = reports
                    .GroupBy(r => r.MemberName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.GroupBy(r => new { r.WeekStart, r.ActivityName }) // Group by WeekStart & ActivityName
                            .Select(subGroup => new ActivityEntry
                            {
                                WeekStart = subGroup.Key.WeekStart,
                                WeekEnd = subGroup.First().WeekEnd, // Assuming all have the same WeekEnd
                                ActivityName = subGroup.Key.ActivityName,
                                Attendance = subGroup.Max(x => x.Attendance) // Take the highest attendance to remove duplicates
                            })
                            .ToList()
                    );


                    /*var result = _mapper.Map<List<AttendanceReportResultVM>>(reports)
                                         .GroupBy(r => new { r.MemberName, r.ActivityName, r.WeekStart })
                                         .Select(g => new AttendanceReportResultVM
                                         {
                                             MemberName = g.Key.MemberName,
                                             ActivityName = g.Key.ActivityName,
                                             WeekStart = g.Key.WeekStart,
                                             WeekEnd = g.First().WeekEnd, // Assuming WeekEnd is always the same within a week
                                             Attendance = g.Max(x => x.Attendance) // Take the highest attendance value if duplicates exist
                                         })
                                         .ToList();*/

                    response.Result = groupedReports;
                    response.Success = true;
                    response.Message = Constants.SuccessResponse;
                }
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