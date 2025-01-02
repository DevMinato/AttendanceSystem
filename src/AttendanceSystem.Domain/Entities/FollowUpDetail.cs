using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceSystem.Domain.Entities
{
    [Table("FollowUpDetails", Schema = "RS")]
    public class FollowUpDetail : BaseEntity
    {
        public Guid FollowUpReportId { get; set; } // Foreign Key to FollowUpReport.Id
        public string FullName { get; set; }
        public string Notes { get; set; } // Notes on the follow-up (e.g., feedback, next steps)

        // Navigation Properties
        public FollowUpReport FollowUpReport { get; set; }
    }
}