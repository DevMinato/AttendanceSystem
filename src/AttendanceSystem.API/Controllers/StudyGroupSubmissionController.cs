using AttendanceSystem.Application.Features.StudyGroup.Commands.Add;
using AttendanceSystem.Application.Features.StudyGroup.Commands.Delete;
using AttendanceSystem.Application.Features.StudyGroup.Commands.Edit;
using AttendanceSystem.Application.Features.StudyGroup.Queries.Get;
using AttendanceSystem.Application.Features.StudyGroup.Queries.GetAll;
using AttendanceSystem.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudyGroupSubmissionController : ControllerBase
    {
        private readonly IMediator _mediator;
        public StudyGroupSubmissionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse>> AddStudyGroupSubmission(AddStudyGroupSubmissionCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("edit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse>> EditStudyGroupSubmission(EditStudyGroupSubmissionCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse>> DeleteStudyGroupSubmission(DeleteStudyGroupSubmissionCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetAllStudyGroupSubmissionsQueryResponse>> GetStudyGroupSubmissions([FromQuery] GetAllStudyGroupSubmissionsQuery query)
        {
            var obj = await _mediator.Send(query);
            return Ok(obj);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetStudyGroupSubmissionQueryResponse>> GetStudyGroupSubmission(Guid id)
        {
            return Ok(await _mediator.Send(new GetStudyGroupSubmissionQuery { Id = id }));
        }
    }
}