namespace AttendanceSystem.Application.Models
{
    public class DocumentRetrievalViewModel
    {
        public string responseCode { get; set; }
        public string responseDescription { get; set; }
        public string document { get; set; }
        public string prefix { get; set; }
        public string fileContentType { get; set; }
    }
}