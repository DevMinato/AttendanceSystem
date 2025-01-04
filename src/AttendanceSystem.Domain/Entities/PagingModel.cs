namespace AttendanceSystem.Domain.Entities
{
    public class PagingModel
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public string SortColumn { get; set; } = "CreatedAt";
        public string SortOrder { get; set; } = "desc";
    }
}