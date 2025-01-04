using AttendanceSystem.Application.Features.Reports.Followup.Commands.Create;
using AttendanceSystem.Application.Features.Reports.Followup.Commands.Delete;
using AttendanceSystem.Application.Features.Reports.Followup.Commands.Edit;
using AttendanceSystem.Application.Features.Reports.Followup.Queries.GetAll;
using AttendanceSystem.Application.Features.Reports.Followup.Queries.GetSingle;
using AttendanceSystem.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FollowupReportController : ControllerBase
    {
        private readonly IMediator _mediator;
        public FollowupReportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse>> CreateReport(CreateFollowupReportCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("edit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse>> EditReport(EditFollowupReportCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse>> DeleteReport(DeleteFollowupReportCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetFollowupReportsQueryResponse>> GetReports([FromQuery] GetFollowupReportsQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetFollowupReportQueryResponse>> GetReport(Guid id)
        {
            return Ok(await _mediator.Send(new GetFollowupReportQuery { ReportId = id }));
        }
    }
}