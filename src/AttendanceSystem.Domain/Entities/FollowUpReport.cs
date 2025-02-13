using AttendanceSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceSystem.Domain.Entities
{
    [Table("FollowUpReports", Schema = "RS")]
    public class FollowUpReport : BaseEntity
    {
        public Guid MemberId { get; set; } // Foreign Key to Member.Id
        public Guid ActivityId { get; set; } // Foreign Key to Activity.Id
        public int TotalFollowUps { get; set; }

        // Navigation Properties
        public Member Member { get; set; }
        public Activity Activity { get; set; }
        public ICollection<FollowUpDetail> FollowUpDetails { get; set; } = new List<FollowUpDetail>();
    }
}