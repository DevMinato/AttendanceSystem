using AttendanceSystem.Application.Features.StandardReports.Queries.Exports.AnalysisReport;
using AttendanceSystem.Application.Features.StandardReports.Queries.Exports.AttendanceReport;
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
        public async Task<FileResult> ExportAttendanceReport([FromQuery] GenerateAttendanceReportQuery filter)
        {
            var fileDto = await _mediator.Send(filter);

            return File(fileDto.Data, fileDto.ContentType, fileDto.ExportFileName);
        }

        [HttpGet("analysis-report/export")]
        public async Task<FileResult> ExportAnalysisReport([FromQuery] GenerateAnalysisReportQuery filter)
        {
            var fileDto = await _mediator.Send(filter);

            return File(fileDto.Data, fileDto.ContentType, fileDto.ExportFileName);
        }
    }
}
