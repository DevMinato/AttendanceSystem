using AttendanceSystem.Application.Features.Reports.Attendance.Commands.Create;
using AttendanceSystem.Application.Features.Reports.Attendance.Commands.Delete;
using AttendanceSystem.Application.Features.Reports.Attendance.Commands.Edit;
using AttendanceSystem.Application.Features.Reports.Attendance.Queries.GetAll;
using AttendanceSystem.Application.Features.Reports.Attendance.Queries.GetSingle;
using AttendanceSystem.Application.Features.Reports.Followup.Commands.Edit;
using AttendanceSystem.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AttendanceReportController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AttendanceReportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse>> CreateReport(CreateAttendanceCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("edit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse>> EditReport(EditAttendanceCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse>> DeleteReport(DeleteAttendanceCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetAttendanceReportsQueryResponse>> GetReports([FromQuery] GetAttendanceReportsQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetAttendanceReportQueryResponse>> GetReport(Guid id)
        {
            return Ok(await _mediator.Send(new GetAttendanceReportQuery { ReportId = id }));
        }
    }
}