using AttendanceSystem.Application.Responses;

namespace AttendanceSystem.Application.Features.Documents.Queries.GetSingle
{
    public class GetDocumentQueryResponse : BaseResponse
    {
        public GetDocumentQueryResponse() : base() { }
        public FileResultVM Result { get; set; } = default!;
    }
}