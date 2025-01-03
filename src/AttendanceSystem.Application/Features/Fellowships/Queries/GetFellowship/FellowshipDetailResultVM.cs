namespace AttendanceSystem.Application.Features.Fellowships.Queries.GetFellowship
{
    public class FellowshipDetailResultVM
    {
        public string Name { get; set; }
        public Guid PastorId { get; set; }
        public string PastorFullName { get; set; }

        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}