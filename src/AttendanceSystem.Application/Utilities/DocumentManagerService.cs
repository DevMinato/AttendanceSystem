using AttendanceSystem.Application.Contracts.Utilities;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Models;
using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Extensions;
using HeyRed.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;

namespace AttendanceSystem.Application.Utilities
{
    public class DocumentManagerService : IDocumentManagerService
    {
        private readonly ILogger<DocumentManagerService> _logger;
        private readonly AppSettings _appSettings;

        public DocumentManagerService(ILogger<DocumentManagerService> logger, IHttpContextAccessor httpContextAccessor, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _appSettings = appSettings.Value;
        }

        public async Task<(string Key, ErrorResponse? ErrorResponse)> UploadDocument(DocumentRequest documentRequest, bool isApprovable = false)
        {
            if (documentRequest.File == null || documentRequest.File.Length == 0)
                throw new CustomException("No file was selected, Please make sure you upload a valid file.");

            var uniqueFileName = GetUniqueFileName(documentRequest.FileName);

            string rootPath = string.Empty;

            if (isApprovable) rootPath = _appSettings.TempResourcePath;
            else rootPath = _appSettings.ResourcePath;

            //_configurationRequest.RootPath
            var safeContentType = documentRequest.DocumentType.DisplayName().Replace('/', '_').Replace('\\', '_');
            var uploadPath = Path.Combine(rootPath, safeContentType);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var extension = Path.GetExtension(documentRequest.FileName);
            var filePath = Path.Combine(uploadPath, $"{Path.GetFileNameWithoutExtension(uniqueFileName)}{extension}");

            try
            {
                byte[] fileBytes = Convert.FromBase64String(documentRequest.File);

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await stream.WriteAsync(fileBytes, 0, fileBytes.Length);
                }

                return (string.Concat(uniqueFileName, extension), null);
            }
            catch (CustomException ex)
            {
                return (string.Empty, new()
                {
                    Errors =
                    [
                        new()
                        {
                            Code = Constants.ErrorCode_SystemError,
                            Message = ex.Message
                        },
                    ],
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return (string.Empty, new()
                {
                    Errors =
                    [
                        new()
                        {
                            Code = Constants.ErrorCode_SystemError,
                            Message = Constants.ErrorResponse,
                        },
                    ],
                });
            }
        }
        private static string ConvertFormFileToString(IFormFile file)
        {
            string imgFile = "";
            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    imgFile = Convert.ToBase64String(fileBytes);
                }
            }

            return AppendTypeToImageFile(imgFile, file.ContentType);
        }
        private static string AppendTypeToImageFile(string imgString, string contentType)
        {
            var sb = new StringBuilder();
            sb.Append(contentType);
            sb.Append(";base64,");
            sb.Append(imgString);
            return sb.ToString();
        }

        public async Task<(DocumentRetrievalViewModel Result, ErrorResponse? ErrorResponse)> GetDocumentByKey(string fileName, string documentType)
        {
            DocumentRetrievalViewModel result = null;
            var error = new ErrorResponse();

            var safeContentType = documentType.Replace('/', '_').Replace('\\', '_');

            string rootPath = _appSettings.ResourcePath;
            var filePath = Path.Combine(rootPath, safeContentType, fileName);

            if (!File.Exists(filePath))
            {
                throw new CustomException("The requested file does not exist.");
            }

            try
            {
                byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
                string base64Document = Convert.ToBase64String(fileBytes);

                // Get the content type from the file extension
                string contentType = MimeTypesMap.GetMimeType(Path.GetExtension(fileName));

                result = new DocumentRetrievalViewModel
                {
                    responseCode = "00",
                    responseDescription = "completed successfully",
                    document = base64Document,
                    prefix = Path.GetFileName(filePath),
                    fileContentType = contentType
                };
            }
            catch (CustomException ex)
            {
                error.Errors.Add(new ErrorModel
                {
                    Code = Constants.ErrorCode_SystemError,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                error.Errors.Add(new ErrorModel
                {
                    Code = Constants.ErrorCode_SystemError,
                    Message = Constants.ErrorResponse
                });
                _logger.LogError(ex.ToString());
            }

            return (result, error);
        }

        public string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return string.Concat(Guid.NewGuid().ToString()?.Replace("-", "")?.ToUpper());

        }
    }
}