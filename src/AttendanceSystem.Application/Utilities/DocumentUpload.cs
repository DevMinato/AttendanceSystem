using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Contracts.Utilities;
using AttendanceSystem.Application.Models;
using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Extensions;
using Microsoft.AspNetCore.Http;

namespace AttendanceSystem.Application.Utilities;

public class DocumentUpload : IDocumentUpload
{
    private readonly IDocumentManagerService _documentManagerService;
    private readonly IAsyncRepository<Document> _documentRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DocumentUpload(IDocumentManagerService documentManagerService, IAsyncRepository<Document> documentRepository, IHttpContextAccessor httpContextAccessor)
    {
        _documentManagerService = documentManagerService;
        _documentRepository = documentRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<(bool Status, ErrorResponse? ErrorResponse)> UploadDocuments(string ownerId, List<DocumentRequest> documentRequests, string clientType)
    {
        List<Document> documents = [];
        foreach (var documentRequest in documentRequests)
        {
            var existingDocument = await _documentRepository.GetSingleAsync(x => x.ClientId == ownerId && x.DocumentType == documentRequest.DocumentType.DisplayName());
            var response = await _documentManagerService.UploadDocument(documentRequest);
            if (response.ErrorResponse != null && response.ErrorResponse.Errors.Count > 0)
            {
                return (false, response.ErrorResponse);
            }

            if (existingDocument == null)
            {
                documents.Add(new()
                {
                    FileName = response.Key,
                    ClientId = ownerId,
                    ClientType = clientType,
                    Id = Guid.NewGuid(),
                    DocumentType = documentRequest.DocumentType.DisplayName(),
                    DocumentName = documentRequest.DocumentName,
                });
            }
            else
            {
                var recordToUpdate = existingDocument.Clone();
                recordToUpdate.FileName = response.Key;
                recordToUpdate.DocumentName = documentRequest.DocumentName;

                await _documentRepository.UpdateAsync(recordToUpdate);
            }
        }
        if (documents.Count > 0)
        {
            await _documentRepository.AddRangeAsync(documents);
        }
        return (true, null);
    }


    private string GetAbsoluteUrl()
    {
        if (_httpContextAccessor.HttpContext == null)
        {
            return "";
        }
        string absoluteUrl = _httpContextAccessor.HttpContext.Request.Path;

        if (_httpContextAccessor.HttpContext.Request.QueryString.HasValue)
        {
            absoluteUrl += _httpContextAccessor.HttpContext.Request.QueryString.Value;
        }
        return absoluteUrl;
    }

}