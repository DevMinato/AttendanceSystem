﻿using AttendanceSystem.Application.Contracts.Infrastructure;
using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Utilities;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Enums;
using AttendanceSystem.Domain.Extensions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.StandardReports.Queries.AnalysisReport
{
    public class GenerateAnalysisReportQueryHandler : IRequestHandler<GenerateAnalysisReportQuery, GenerateAnalysisReportQueryResponse>
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

        public async Task<GenerateAnalysisReportQueryResponse> Handle(GenerateAnalysisReportQuery request, CancellationToken cancellationToken)
        {
            var response = new GenerateAnalysisReportQueryResponse();
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

                    var result = _mapper.Map<List<AnalysisReportResultVM>>(reports);

                    response.Result = result;
                    response.Success = true;
                    response.Message = Constants.SuccessResponse;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex.ToString() + "{Microservice}", Constants.Microservice);
                response.Message = $"Error completing fellowship analysis querying request.";
            }
            catch (CustomException ex)
            {
                _logger.LogError(ex.ToString(), ex.Message);
                response.Message = ex.Message;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response.Success = false;
                response.Message = Constants.ErrorResponse;
            }
            return response;
        }


    }
}