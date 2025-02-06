using AttendanceSystem.Application.Contracts.Infrastructure;
using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Models;
using AttendanceSystem.Application.Utilities;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Enums;
using AttendanceSystem.Domain.Extensions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.StandardReports.Queries.Exports.AnalysisReport
{
    public class GenerateAnalysisReportQueryHandler : IRequestHandler<GenerateAnalysisReportQuery, AnalysisReportExportFileVm>
    {
        private readonly ILogger<GenerateAnalysisReportQueryHandler> _logger;
        private readonly IUserService _userService;
        private readonly IExcelExporter _excelExporter;
        private readonly IWordExporter _wordExporter;
        private readonly IPdfExporter _pdfExporter;
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Fellowship> _fellowshipRepository;

        public GenerateAnalysisReportQueryHandler(ILogger<GenerateAnalysisReportQueryHandler> logger, IUserService userService, IExcelExporter excelExporter,
            IPdfExporter pdfExporter, IReportRepository reportRepository, IMapper mapper, IWordExporter wordExporter, IAsyncRepository<Fellowship> fellowshipRepository)
        {
            _logger = logger;
            _userService = userService;
            _excelExporter = excelExporter;
            _pdfExporter = pdfExporter;
            _reportRepository = reportRepository;
            _mapper = mapper;
            _wordExporter = wordExporter;
            _fellowshipRepository = fellowshipRepository;
        }

        public async Task<AnalysisReportExportFileVm> Handle(GenerateAnalysisReportQuery request, CancellationToken cancellationToken)
        {
            var exportFileDto = new AnalysisReportExportFileVm();
            string fellowshipName = string.Empty;
            Guid? fellowshipId = Guid.Empty;
            List<Guid> activityIds = new List<Guid>();

            try
            {
                if (_userService?.UserDetails()?.UserType == MemberType.Pastor.DisplayName())
                {

                    if (!request.StartDate.HasValue)
                        request.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    if (!request.EndDate.HasValue)
                        request.EndDate = DateTime.Now;

                    if (request.FellowshipId.HasValue)
                    {
                        var fellowship = await _fellowshipRepository.GetSingleAsync(_ => _.Id == request.FellowshipId);
                        if (fellowship != null) fellowshipName = fellowship.Name;
                    }
                    else
                    {
                        fellowshipId = _userService?.UserDetails()?.GroupId;
                        fellowshipName = _userService?.UserDetails()?.GroupName ?? string.Empty;
                    }

                    if (!string.IsNullOrEmpty(request.ActivityIds))
                    {
                        activityIds = request.ActivityIds
                            .Split(',')
                            .Select(Guid.Parse)
                            .ToList();
                    }

                    var reports = await _reportRepository.FetchMonthlyAttendanceReportAsync(request.StartDate.Value, request.EndDate.Value, fellowshipId, activityIds);

                    exportFileDto = await GetExportRecord(request.StartDate.Value, request.EndDate.Value, fellowshipName, request.Period.DisplayName(), reports, request.ExportType);
                }
            }
            catch (Exception ex)
            {
                exportFileDto = await GetExportRecord(request.StartDate.Value, request.EndDate.Value, fellowshipName, request.Period.DisplayName(), new List<MonthlyAttendanceReportViewModel>(), request.ExportType);
                _logger.LogError(ex, ex.Message);
            }
            return exportFileDto;
        }


        private async Task<AnalysisReportExportFileVm> GetExportRecord(DateTime startDate, DateTime endDate, string fellowshipName, string period, List<MonthlyAttendanceReportViewModel> data, string ExportType)
        {
            string contentType = string.Empty;
            string fileExtension = string.Empty;
            byte[] fileData = default!;

            switch (ExportType)
            {
                case Constants.ExportTypeWord:
                    contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    fileExtension = "docx";
                    fileData = await _wordExporter.ExportAttendanceReportToWordAsync(startDate, endDate, fellowshipName, period, data);
                    break;
                case Constants.ExportTypePdf:
                    contentType = "application/pdf";
                    fileExtension = "pdf";
                    //fileData = _pdfExporter.ExportTransactionsToPdf(data);
                    break;
            }

            string fileName = $"{Guid.NewGuid()}.{fileExtension}";
            var exportFileDto = new AnalysisReportExportFileVm
            {
                ContentType = contentType,
                Data = fileData,
                ExportFileName = fileName
            };

            return exportFileDto;
        }
    }
}