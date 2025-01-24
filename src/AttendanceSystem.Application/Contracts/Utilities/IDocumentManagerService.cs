using AttendanceSystem.Application.Models;
using AttendanceSystem.Application.Responses;

namespace AttendanceSystem.Application.Contracts.Utilities
{
    public interface IDocumentManagerService
    {
        Task<(DocumentRetrievalViewModel Result, ErrorResponse? ErrorResponse)> GetDocumentByKey(string fileName, string documentType);
        Task<(string Key, ErrorResponse? ErrorResponse)> UploadDocument(DocumentRequest documentRequest, bool isApprovable = false);
    }
}