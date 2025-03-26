using AttendanceSystem.Application.Features.Analytics.Queries.AttendanceStatistics;
using AttendanceSystem.Application.Features.Analytics.Queries.PerformanceAnalysis;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AnalyticsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AnalyticsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("attendance-statistics")]
        public async Task<ActionResult<GetAttendanceStatisticsQueryResponse>> GetAttendanceStatistics([FromQuery] GetAttendanceStatisticsQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("performance-analysis")]
        public async Task<ActionResult<GetAttendanceStatisticsQueryResponse>> GetPerformanceAnalysisStatistics([FromQuery] GetPerformanceAnalysisQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
    }
}