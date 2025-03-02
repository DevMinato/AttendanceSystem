namespace AttendanceSystem.Application.Features.Fellowships.Queries.GetAllFellowships
{
    public class FellowshipsListResultVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid PastorId { get; set; }
        public Guid? ParentId { get; set; }
        public string? ParentFellowship { get; set; }
        public string PastorFullName { get; set; }
    }
}