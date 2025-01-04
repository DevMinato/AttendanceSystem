using AttendanceSystem.Application.Features.Members.Commands.AddMember;
using AttendanceSystem.Application.Features.Members.Commands.DeleteMember;
using AttendanceSystem.Application.Features.Members.Commands.EditMember;
using AttendanceSystem.Application.Features.Members.Queries.GetAllMembers;
using AttendanceSystem.Application.Features.Members.Queries.GetMember;
using AttendanceSystem.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MemberController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MemberController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AddMemberCommandResponse>> AddMember(AddMemberCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("edit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse>> EditMember(EditMemberCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse>> DeleteMember(DeleteMemberCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetAllMembersQueryResponse>> GetMembers([FromQuery] GetAllMembersQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetMemberQueryResponse>> GetMember(Guid id)
        {
            return Ok(await _mediator.Send(new GetMemberQuery { Id = id }));
        }
    }
}