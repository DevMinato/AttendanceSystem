using AttendanceSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceSystem.Domain.Entities
{
    [Table("Activities", Schema = "RS")]
    public class Activity : BaseEntity
    {
        public string Name { get; set; } // E.g., Outreach, Follow-Up, Prayer Meeting
        public string Description { get; set; }
        public ActivityType Type { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public ICollection<ActivityReport> ActivityReports { get; set; }
    }
}