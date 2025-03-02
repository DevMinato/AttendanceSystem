using AttendanceSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceSystem.Domain.Entities
{
    [Table("FollowUpDetails", Schema = "RS")]
    public class FollowUpDetail : BaseEntity
    {
        public Guid FollowUpReportId { get; set; } 
        public Guid MemberId { get; set; }
        public FollowUpType FollowUpType { get; set; }
        public Guid DiscipleId { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime? Date { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public FollowUpReport FollowUpReport { get; set; }
    }
}