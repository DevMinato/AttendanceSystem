using AttendanceSystem.Application.Features.Activities.Queries.GetActivity;
using AttendanceSystem.Application.Features.Activities.Queries.GetAllActivities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ActivityController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ActivityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetAllActivitiesQueryResponse>> GetActivities([FromQuery] GetAllActivitiesQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetActivityQueryResponse>> GetActivity(Guid id)
        {
            return Ok(await _mediator.Send(new GetActivityQuery { Id = id }));
        }
    }
}