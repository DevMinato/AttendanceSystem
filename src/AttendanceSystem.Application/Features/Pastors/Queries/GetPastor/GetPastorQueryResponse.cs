using AttendanceSystem.Application.Responses;

namespace AttendanceSystem.Application.Features.Pastors.Queries.GetPastor
{
    public class GetPastorQueryResponse : BaseResponse
    {
        public GetPastorQueryResponse() : base() { }
        public PastorDetailResultVM Result { get; set; } = default!;
    }
}