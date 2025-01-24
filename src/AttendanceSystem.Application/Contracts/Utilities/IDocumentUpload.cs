using AttendanceSystem.Application.Models;
using AttendanceSystem.Application.Responses;

namespace AttendanceSystem.Application.Contracts.Utilities;

public interface IDocumentUpload
{
    Task<(bool Status, ErrorResponse? ErrorResponse)> UploadDocuments(string pspId, List<DocumentRequest> documentRequests, string clientType);
}