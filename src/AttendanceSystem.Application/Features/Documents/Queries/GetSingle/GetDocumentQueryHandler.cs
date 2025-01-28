using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Contracts.Utilities;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Documents.Queries.GetSingle
{
    public class GetDocumentQueryHandler : IRequestHandler<GetDocumentQuery, GetDocumentQueryResponse>
    {
        private readonly ILogger<GetDocumentQueryHandler> _logger;
        private readonly IAsyncRepository<Document> _documentRepository;
        private readonly IMapper _mapper;
        private readonly IDocumentManagerService _documentManagerService;
        public GetDocumentQueryHandler(ILogger<GetDocumentQueryHandler> logger, IAsyncRepository<Document> documentRepository,
            IMapper mapper, IDocumentManagerService documentManagerService)
        {
            _logger = logger;
            _documentRepository = documentRepository;
            _mapper = mapper;
            _documentManagerService = documentManagerService;
        }

        public async Task<GetDocumentQueryResponse> Handle(GetDocumentQuery request, CancellationToken cancellationToken)
        {
            var response = new GetDocumentQueryResponse();
            try
            {
                var document = await _documentRepository.GetSingleAsync(x => x.ClientId == request.RecordId.ToString(), false);
                if (document == null)
                {
                    throw new CustomException(Constants.ErrorCode_RecordNotFound + $" Document with request id {request.RecordId} not found.");
                }

                var result = await SetDocumentByName(document.FileName, document.DocumentType, document.DocumentName);

                response.Result = result;
                response.Success = true;
                response.Message = Constants.SuccessResponse;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex.ToString() + "{Microservice}", Constants.Microservice);
                response.Message = $"Error completing fellowship querying request.";
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

        private async Task<FileResultVM> SetDocumentByName(string fileName, string documentType, string documentName)
        {
            FileResultVM data = new FileResultVM();

            try
            {
                var doc = await _documentManagerService.GetDocumentByKey(fileName, documentType);
                if (doc.Item2.Errors.Count == 0)
                {
                    var result = new FileResultVM();
                    result.contentType = doc.Item1.fileContentType;
                    result.document = doc.Item1.document;
                    result.fileName = doc.Item1.prefix;
                    result.documentName = documentName;

                    data = result;
                }

            }
            catch (Exception ex)
            {
                data = null;
                _logger.LogError(ex.ToString());
            }

            return data;
        }

    }
}