namespace AttendanceSystem.Domain.Entities
{
    public class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}