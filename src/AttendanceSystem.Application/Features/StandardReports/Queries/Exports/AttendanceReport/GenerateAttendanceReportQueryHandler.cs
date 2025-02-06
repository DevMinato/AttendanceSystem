using AttendanceSystem.Application.Contracts.Infrastructure;
using AttendanceSystem.Application.Models;
using AttendanceSystem.Application.Utilities;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Enums;
using AttendanceSystem.Domain.Extensions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.StandardReports.Queries.Exports.AttendanceReport
{
    public class GenerateAttendanceReportQueryHandler : IRequestHandler<GenerateAttendanceReportQuery, AttendanceReportExportFileVm>
    {
        private readonly ILogger<GenerateAttendanceReportQueryHandler> _logger;
        private readonly IUserService _userService;
        private readonly IExcelExporter _excelExporter;
        private readonly IPdfExporter _pdfExporter;
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;

        public GenerateAttendanceReportQueryHandler(ILogger<GenerateAttendanceReportQueryHandler> logger, IUserService userService, IExcelExporter excelExporter, 
            IPdfExporter pdfExporter, IReportRepository reportRepository, IMapper mapper)
        {
            _logger = logger;
            _userService = userService;
            _excelExporter = excelExporter;
            _pdfExporter = pdfExporter;
            _reportRepository = reportRepository;
            _mapper = mapper;
        }

        public async Task<AttendanceReportExportFileVm> Handle(GenerateAttendanceReportQuery request, CancellationToken cancellationToken)
        {
            var exportFileDto = new AttendanceReportExportFileVm();
            List<Guid> activityIds = new List<Guid>();

            try
            {

                if (_userService?.UserDetails()?.UserType == MemberType.Pastor.DisplayName())
                {

                    if (!request.StartDate.HasValue)
                        request.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    if (!request.EndDate.HasValue)
                        request.EndDate = DateTime.Now;

                    if (!string.IsNullOrEmpty(request.ActivityIds))
                    {
                        activityIds = request.ActivityIds
                            .Split(',')
                            .Select(Guid.Parse)
                            .ToList();
                    }

                    var reports = await _reportRepository.FetchAttendanceData(request.StartDate.Value, request.EndDate.Value, activityIds);

                    exportFileDto = await GetExportRecord(reports, request.ExportType);
                }
            }
            catch (Exception ex)
            {
                exportFileDto = await GetExportRecord(new List<AttendanceReportViewModel>(), request.ExportType);
                _logger.LogError(ex, ex.Message);
            }
            return exportFileDto;
        }


        private async Task<AttendanceReportExportFileVm> GetExportRecord(List<AttendanceReportViewModel> data, string ExportType)
        {
            string contentType = string.Empty;
            string fileExtension = string.Empty;
            byte[] fileData = default!;

            var mappedData = _mapper.Map<List<AttendanceReportExportDto>>(data);

            switch (ExportType)
            {
                case Constants.ExportTypeExcel:
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    fileExtension = "xlsx";
                    fileData = _excelExporter.ExportAttendanceReportsToExcel(mappedData);
                    break;
                case Constants.ExportTypePdf:
                    contentType = "application/pdf";
                    fileExtension = "pdf";
                    //fileData = _pdfExporter.ExportTransactionsToPdf(data);
                    break;
            }

            string fileName = $"{Guid.NewGuid()}.{fileExtension}";
            var exportFileDto = new AttendanceReportExportFileVm
            {
                ContentType = contentType,
                Data = fileData,
                ExportFileName = fileName
            };

            return exportFileDto;
        }
    }
}