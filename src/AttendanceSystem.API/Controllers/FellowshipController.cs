using AttendanceSystem.Application.Features.Fellowships.Commands.AddFellowship;
using AttendanceSystem.Application.Features.Fellowships.Commands.DeleteFellowship;
using AttendanceSystem.Application.Features.Fellowships.Commands.EditFellowship;
using AttendanceSystem.Application.Features.Fellowships.Queries.GetAllFellowships;
using AttendanceSystem.Application.Features.Fellowships.Queries.GetFellowship;
using AttendanceSystem.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FellowshipController : ControllerBase
    {
        private readonly IMediator _mediator;
        public FellowshipController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AddFellowshipCommandResponse>> AddFellowship(AddFellowshipCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("edit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse>> EditFellowship(EditFellowshipCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse>> DeleteFellowship(DeleteFellowshipCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<ActionResult<GetAllFellowshipsQueryResponse>> GetFellowships([FromQuery] GetAllFellowshipsQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<ActionResult<GetFellowshipQueryResponse>> GetFellowship(Guid id)
        {
            return Ok(await _mediator.Send(new GetFellowshipQuery { Id = id }));
        }
    }
}