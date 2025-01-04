using AttendanceSystem.Application.Features.Reports.Outreach.Commands.Create;
using AttendanceSystem.Application.Features.Reports.Outreach.Commands.Delete;
using AttendanceSystem.Application.Features.Reports.Outreach.Commands.Edit;
using AttendanceSystem.Application.Features.Reports.Outreach.Queries.GetAll;
using AttendanceSystem.Application.Features.Reports.Outreach.Queries.GetSingle;
using AttendanceSystem.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OutreachReportController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OutreachReportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse>> CreateReport(CreateOutreachReportCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("edit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse>> EditReport(EditOutreachReportCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse>> DeleteReport(DeleteOutreachReportCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetOutreachReportsQueryResponse>> GetReports([FromQuery] GetOutreachReportsQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetOutreachReportQueryResponse>> GetReport(Guid id)
        {
            return Ok(await _mediator.Send(new GetOutreachReportQuery { ReportId = id }));
        }
    }
}