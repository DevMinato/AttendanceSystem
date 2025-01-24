using AttendanceSystem.Domain.Enums;

namespace AttendanceSystem.Application.Features.Setups.Activities.Queries.GetActivity
{
    public class ActivityDetailResultVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ActivityType Type { get; set; }

        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}