namespace AttendanceSystem.Application.Responses
{
    public class ErrorResponse
    {
        public ErrorResponse()
        {
            Errors = new List<ErrorModel>();
        }

        public List<ErrorModel> Errors { get; set; }
    }

    public class ErrorModel
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}