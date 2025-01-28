using Amazon.Runtime.Internal;
using MediatR;

namespace AttendanceSystem.Application.Features.Documents.Queries.GetSingle
{
    public class GetDocumentQuery : IRequest<GetDocumentQueryResponse>
    {
        public Guid RecordId { get; set; }
    }
}