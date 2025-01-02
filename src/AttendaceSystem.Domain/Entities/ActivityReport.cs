using AttendanceSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceSystem.Domain.Entities
{
    [Table("ActivityReports", Schema = "RS")]
    public class ActivityReport : BaseEntity
    {
        public Guid MemberId { get; set; } // Foreign Key to Member.Id
        public Guid ActivityId { get; set; } // Foreign Key to Activity.Id
        public string Description { get; set; }
        public DateTime Date { get; set; } // Date of the activity

        // Navigation Properties
        public Member Member { get; set; }
        public Activity Activity { get; set; }
    }
}