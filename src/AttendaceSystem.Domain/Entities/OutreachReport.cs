using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceSystem.Domain.Entities
{
    [Table("OutreachReports", Schema = "RS")]
    public class OutreachReport : BaseEntity
    {
        public Guid MemberId { get; set; } // Foreign Key to Member.Id
        public Guid ActivityId { get; set; } // Foreign Key to Activity.Id
        public int TotalPeopleReached { get; set; }
        public string Notes { get; set; } // Additional notes or details
        public DateTime Date { get; set; }

        // Navigation Properties
        public Member Member { get; set; }
        public Activity Activity { get; set; }
        public ICollection<OutreachDetail> OutreachDetails { get; set; }
    }
}