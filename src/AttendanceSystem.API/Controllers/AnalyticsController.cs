using AttendanceSystem.Application.Features.Analytics.Queries.AttendanceStatistics;
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
    }
}