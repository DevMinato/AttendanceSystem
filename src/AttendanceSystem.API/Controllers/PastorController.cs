using AttendanceSystem.Application.Features.Pastors.Commands.AddPastor;
using AttendanceSystem.Application.Features.Pastors.Commands.DeletePastor;
using AttendanceSystem.Application.Features.Pastors.Commands.EditPastor;
using AttendanceSystem.Application.Features.Pastors.Queries.GetAllPastors;
using AttendanceSystem.Application.Features.Pastors.Queries.GetPastor;
using AttendanceSystem.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PastorController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PastorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AddPastorCommandResponse>> AddPastor(AddPastorCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("edit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse>> EditPastor(EditPastorCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse>> DeletePastor(DeletePastorCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetAllPastorsQueryResponse>> GetPastors([FromQuery] GetAllPastorsQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetPastorQueryResponse>> GetPastor(Guid id)
        {
            return Ok(await _mediator.Send(new GetPastorQuery { Id = id }));
        }
    }
}