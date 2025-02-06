using AttendanceSystem.Application.Features.StandardReports.Queries.AnalysisReport;
using AttendanceSystem.Application.Features.StandardReports.Queries.AttendanceReport;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StandardReportController : ControllerBase
    {
        private readonly IMediator _mediator;
        public StandardReportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("attendance-report/export")]
        public async Task<FileResult> ExportAttendanceReport([FromQuery] Application.Features.StandardReports.Queries.Exports.AttendanceReport.GenerateAttendanceReportQuery filter)
        {
            var fileDto = await _mediator.Send(filter);

            return File(fileDto.Data, fileDto.ContentType, fileDto.ExportFileName);
        }

        [HttpGet("analysis-report/export")]
        public async Task<FileResult> ExportAnalysisReport([FromQuery] Application.Features.StandardReports.Queries.Exports.AnalysisReport.GenerateAnalysisReportQuery filter)
        {
            var fileDto = await _mediator.Send(filter);

            return File(fileDto.Data, fileDto.ContentType, fileDto.ExportFileName);
        }

        [HttpGet("attendance-report")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GenerateAttendanceReportQueryResponse>> GetAttendanceReport([FromQuery] 
        Application.Features.StandardReports.Queries.AttendanceReport.GenerateAttendanceReportQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("analysis-report")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GenerateAnalysisReportQueryResponse>> GetDailyTransactionTypeStatistics([FromQuery] 
        Application.Features.StandardReports.Queries.AnalysisReport.GenerateAnalysisReportQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
    }
}