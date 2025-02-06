using AttendanceSystem.Application.Contracts.Infrastructure;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Utilities;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Enums;
using AttendanceSystem.Domain.Extensions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Analytics.Queries.AttendanceStatistics
{
    public class GetAttendanceStatisticsQueryHandler : IRequestHandler<GetAttendanceStatisticsQuery, GetAttendanceStatisticsQueryResponse>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<GetAttendanceStatisticsQueryHandler> _logger;
        private readonly IUserService _userService;
        private readonly IAttendanceStatisticsRepository _attendanceStatisticsRepository;

        public GetAttendanceStatisticsQueryHandler(IMapper mapper, ILogger<GetAttendanceStatisticsQueryHandler> logger, 
            IUserService userService, IAttendanceStatisticsRepository attendanceStatisticsRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _logger = logger;
            _userService = userService;
            _attendanceStatisticsRepository = attendanceStatisticsRepository;
        }

        public async Task<GetAttendanceStatisticsQueryResponse> Handle(GetAttendanceStatisticsQuery request, CancellationToken cancellationToken)
        {
            var response = new GetAttendanceStatisticsQueryResponse();
            List<Guid> activityIds = new List<Guid>();

            try
            {
                if (!request.StartDate.HasValue)
                    request.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                if (!request.EndDate.HasValue)
                    request.EndDate = DateTime.Now;

                request.EndDate = request.EndDate.Value.Date.AddDays(1).AddSeconds(-1);

                if (_userService.UserDetails().UserType == MemberType.WorkersInTraining.DisplayName())
                    request.MemberId = _userService.UserDetails().UserId;
                else if (_userService?.UserDetails()?.UserType == MemberType.Pastor.DisplayName())
                    request.FellowshipId = _userService.UserDetails().GroupId;

                if (!string.IsNullOrEmpty(request.ActivityIds))
                {
                    activityIds = request.ActivityIds
                        .Split(',')
                        .Select(Guid.Parse)
                        .ToList();
                }

                var result = await _attendanceStatisticsRepository.GetAttendanceStatisticsAsync(
                    request.StartDate.Value, request.EndDate.Value, request.FellowshipId, activityIds, request.MemberId);

                response.Result = result;
                response.Success = true;
                response.Message = Constants.SuccessResponse;
            }
            catch (CustomException ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.Success = false;
                response.Message = Constants.ErrorResponse;
            }
            return response;
        }
    }
}