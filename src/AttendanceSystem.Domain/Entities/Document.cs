using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceSystem.Domain.Entities;

[Table("Document")]
public class Document : BaseEntity
{
    public Document Clone()
    {
        return (Document)MemberwiseClone();
    }

    public string DocumentType { get; set; } = string.Empty;
    public string? DocumentName { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string ClientType { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string IdentificationNumber { get; set; } = string.Empty;
    public string? UploadStatus { get; set; }
}