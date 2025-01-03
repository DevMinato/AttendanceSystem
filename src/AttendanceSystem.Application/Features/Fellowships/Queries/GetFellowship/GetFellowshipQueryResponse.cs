using AttendanceSystem.Application.Responses;

namespace AttendanceSystem.Application.Features.Fellowships.Queries.GetFellowship
{
    public class GetFellowshipQueryResponse : BaseResponse
    {
        public GetFellowshipQueryResponse() : base() { }
        public FellowshipDetailResultVM Result { get; set; } = default!;
    }
}