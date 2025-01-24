using AttendanceSystem.Domain.Enums;

namespace AttendanceSystem.Application.Models
{
    public class DocumentRequest
    {
        public string File { get; set; }
        public string FileName { get; set; }
        public string DocumentName { get; set; }
        public DocumentType DocumentType { get; set; }
        public string ContentType { get; set; }
    }
}