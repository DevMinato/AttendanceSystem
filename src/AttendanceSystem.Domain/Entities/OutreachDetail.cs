using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceSystem.Domain.Entities
{
    [Table("OutreachDetails", Schema = "RS")]
    public class OutreachDetail : BaseEntity
    {
        public Guid OutreachReportId { get; set; } // Foreign Key to OutreachReport.Id
        public string FullName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        // Navigation Properties
        public OutreachReport OutreachReport { get; set; }
    }
}