using AttendanceSystem.Application.Features.Auths.Commands.ApproveUser;
using AttendanceSystem.Application.Features.Auths.Commands.CreateUser;
using AttendanceSystem.Application.Features.Auths.Commands.LoginUser;
using AttendanceSystem.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse>> Register([FromBody] CreateUserCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }


        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<LoginUserCommandResponse>> Login([FromBody] LoginUserCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("approval")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse>> UserApproval(ApproverUserCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
