using AttendanceSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceSystem.Domain.Entities
{
    [Table("FollowUpReports", Schema = "RS")]
    public class FollowUpReport : BaseEntity
    {
        public Guid MemberId { get; set; } // Foreign Key to Member.Id
        public Guid ActivityId { get; set; } // Foreign Key to Activity.Id
        public FollowUpType FollowUpType { get; set; } // Enum: Visitation, Teaching
        public int TotalFollowUps { get; set; } // Total number of follow-ups
        public string Notes { get; set; } // Additional notes or details
        public DateTime Date { get; set; }

        // Navigation Properties
        public Member Member { get; set; }
        public Activity Activity { get; set; }
        public ICollection<FollowUpDetail> FollowUpDetails { get; set; }
    }
}