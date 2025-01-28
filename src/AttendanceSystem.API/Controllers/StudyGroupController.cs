using AttendanceSystem.Application.Features.Setups.StudyGroup.Commands.Add;
using AttendanceSystem.Application.Features.Setups.StudyGroup.Commands.Delete;
using AttendanceSystem.Application.Features.Setups.StudyGroup.Commands.Edit;
using AttendanceSystem.Application.Features.Setups.StudyGroup.Queries.GetAllStudyGroups;
using AttendanceSystem.Application.Features.Setups.StudyGroup.Queries.GetStudyGroup;
using AttendanceSystem.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudyGroupController : ControllerBase
    {
        private readonly IMediator _mediator;
        public StudyGroupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AddStudyGroupCommandResponse>> AddStudyGroup(AddStudyGroupCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("edit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse>> EditStudyGroup(EditStudyGroupCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse>> DeleteStudyGroup(DeleteStudyGroupCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetAllStudyGroupsQueryResponse>> GetStudyGroups([FromQuery] GetAllStudyGroupsQuery query)
        {
            var obj = await _mediator.Send(query);
            return Ok(obj);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetStudyGroupQueryResponse>> GetStudyGroup(Guid id)
        {
            return Ok(await _mediator.Send(new GetStudyGroupQuery { Id = id }));
        }
    }
}