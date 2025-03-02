using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceSystem.Domain.Entities
{
    [Table("Fellowships", Schema = "RS")]
    public class Fellowship : BaseEntity
    {
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public Guid PastorId { get; set; }
    }
}
