using AttendanceSystem.Domain.Enums;

namespace AttendanceSystem.Domain.Entities
{
    public class AuditableEntity
    {
        public ApprovalStatus Status { get; set; }
        public string CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}