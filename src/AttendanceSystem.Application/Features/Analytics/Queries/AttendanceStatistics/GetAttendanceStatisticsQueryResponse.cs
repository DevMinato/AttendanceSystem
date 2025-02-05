using AttendanceSystem.Application.Models;
using AttendanceSystem.Application.Responses;

namespace AttendanceSystem.Application.Features.Analytics.Queries.AttendanceStatistics
{
    public class GetAttendanceStatisticsQueryResponse : BaseResponse
    {
        public GetAttendanceStatisticsQueryResponse()
        {
        }

        public List<AttendanceStatisticsViewModel> Result { get; set; }
    }
}
