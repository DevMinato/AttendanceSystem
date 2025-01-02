using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceSystem.Domain.Entities
{
    [Table("AttendanceReports", Schema = "RS")]
    public class AttendanceReport : BaseEntity
    {
        public Guid MemberId { get; set; } // Foreign Key to Member.Id
        public Guid ActivityId { get; set; } // Foreign Key to Activity.Id
        public bool IsPresent { get; set; } // True if present, false if absent
        public DateTime Date { get; set; } // Date of the activity

        // Navigation Properties
        public Member Member { get; set; }
        public Activity Activity { get; set; }
    }
}
